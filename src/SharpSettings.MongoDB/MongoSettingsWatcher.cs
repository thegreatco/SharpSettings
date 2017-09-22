using System;
using System.Threading;
using System.Threading.Tasks;
using KellermanSoftware.CompareNetObjects;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Clusters;

namespace SharpSettings.MongoDB
{
    public class MongoSettingsWatcher<TSettingsObject> : ISettingsWatcher<string, TSettingsObject> where TSettingsObject : MongoWatchableSettings
    {
        private readonly string _settingsId;
        private readonly CompareLogic _compareLogic;
        private readonly MongoDataStore<TSettingsObject> _store;
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        private Task _watcherTask;
        private readonly Action<TSettingsObject> _settingsUpdatedCallback;

        private TSettingsObject _settings;

        public async Task<TSettingsObject> GetSettingsAsync()
        {
            var token = _cts.Token;
            while(_settings == null && !token.IsCancellationRequested)
            {
                await Task.Delay(100, token);
            }
            return _settings;
        }

        public MongoSettingsWatcher(MongoDataStore<TSettingsObject> settingsStore, MongoWatchableSettings settings, Action<TSettingsObject> settingsUpdatedCallback, bool forcePolling = false)
            : this(settingsStore, settings.Id, settingsUpdatedCallback, forcePolling)
        {
        }

        public MongoSettingsWatcher(MongoDataStore<TSettingsObject> settingsStore, string settingsId, Action<TSettingsObject> settingsUpdatedCallback, bool forcePolling = false)
        {
            _compareLogic = new CompareLogic();
            _store = settingsStore;
            _settingsId = settingsId;
            _settingsUpdatedCallback = settingsUpdatedCallback;
            if (_store.Store.Database.Client.Cluster.Description.Type == ClusterType.ReplicaSet && forcePolling == false)
            {
                _store.Logger?.Debug("Calling start on a Polling task.");
                _watcherTask = Task.Factory.StartNew(TailAsync, _cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
                _store.Logger?.Debug("Finished calling start on a Polling task.");
            }
            else
            {
                _store.Logger?.Debug("Calling start on a Watcher task.");
                _watcherTask = Task.Factory.StartNew(PollAsync, _cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
                _store.Logger?.Debug("Finished calling start on a Watcher task.");
            }
        }

        private async Task PollAsync()
        {
            _store.Logger?.Trace("Starting a Polling task.");
            while (!_cts.IsCancellationRequested)
            {
                try
                {
                    if (_store.Store.Database.Client.Cluster.Description.Type == ClusterType.ReplicaSet)
                    {
                        _store.Logger?.Trace("Detected change to replica set. Changing to OpLog Tail.");

                        _watcherTask = Task.Factory.StartNew(TailAsync, _cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
                        break;
                    }
                    var tmpSettings = await _store.FindAsync(_settingsId);

                    if (_compareLogic.Compare(tmpSettings, _settings).AreEqual == false)
                    {
                        _store.Logger?.Trace("Settings updated.");

                        _settings = tmpSettings;
                        _settingsUpdatedCallback?.Invoke(_settings);

                        _store.Logger?.Trace("SettingsWatcher notified.");
                    }
                    if (_settings == null)
                    {
                        _store.Logger?.Warn("Settings not found.");
                    }
                }
                catch (Exception ex)
                {
                    _store.Logger?.Error(ex);
                }
                await Task.Delay(500);
            }
            _store.Logger?.Trace("Ending a Polling task.");
        }

        private async Task TailAsync()
        {
            _store.Logger?.Trace("Starting a Tailing task.");
            var token = _cts.Token;
            _store.Logger?.Trace("Doing initial setting read");
            _settings = await _store.FindAsync(_settingsId);
            if(_settings == null)
            {
                throw new Exception("Failed to do intial settings load.");
            }
            
            _store.Logger?.Trace("Initial settings loaded.");
            var localDb = _store.Store.Database.Client.GetDatabase("local");
            var oplog = localDb.GetCollection<BsonDocument>("oplog.rs");
            while (!token.IsCancellationRequested)
            {
                try
                {
                    if (_store.Store.Database.Client.Cluster.Description.Type != ClusterType.ReplicaSet)
                    {
                        _store.Logger?.Trace("Detected change to non-replica set. Changing to Poll.");
                        _watcherTask = Task.Factory.StartNew(PollAsync, _cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
                        break;
                    }

                    var cursor = await oplog.Find(Builders<BsonDocument>.Filter.And(
                            Builders<BsonDocument>.Filter.Eq("ns", _store.Store.CollectionNamespace.FullName),
                            Builders<BsonDocument>.Filter.Eq("o2", new BsonDocument("_id", _settingsId))),
                        new FindOptions()
                        {
                            CursorType = CursorType.TailableAwait,
                            Comment = "MongoSettingsdWatcher cursor."
                        }).ToCursorAsync(token);
                    while (await cursor.MoveNextAsync(token))
                    {
                        foreach (var update in cursor.Current)
                        {
                            if (new DateTime(1970, 1, 1).AddSeconds(update["ts"].AsBsonTimestamp.Timestamp) < DateTime.UtcNow.AddSeconds(-10)) continue;

                            _store.Logger?.Trace("Settings updated.");

                            _settings = await _store.FindAsync(_settingsId);
                            _settingsUpdatedCallback?.Invoke(_settings);

                            _store.Logger?.Trace("SettingsWatcher notified.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    _store.Logger?.Error(ex);
                }
            }
            _store.Logger?.Trace("Ending a Tailing task.");
        }

        public void Dispose()
        {
            _cts.Cancel();
            _watcherTask.Wait(TimeSpan.FromSeconds(10));
        }
    }
}
