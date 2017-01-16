using System;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace SharpSettings
{
    public class MongoSettingsWatcher<TSettings> : SettingsWatcher<TSettings, ObjectId> where TSettings : WatchableSettings<ObjectId>
    {
        public MongoSettingsWatcher(IDataStore<TSettings, ObjectId> settingsStore, ObjectId settingsId) : base(settingsStore, settingsId)
        {
        }

        // public void StartWatch()
        // {
        //     _watcherTask = Task.Factory.StartNew(RunWatchAsync, _cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        // }

        // public void StopWatch()
        // {
        //     _stopWatcher = true;
        //     var watcherStopped = _watcherTask.Wait(10000);
        //     if (watcherStopped) return;

        //     _cts.Cancel(true);
        //     watcherStopped = _watcherTask.Wait(10000);
        //     if (watcherStopped == false)
        //         throw new Exception("Failed to stop SettingsWatcher.");
        // }

        // private async Task RunWatchAsync()
        // {
        //     while (!_stopWatcher)
        //     {
        //         _settings = await _store.FindAsync(SettingsId);
        //         if(_settings == null)
        //             throw new Exception("Settings not found.");
        //         Console.WriteLine("Settings updated.");
        //         await Task.Delay(1000);
        //     }
        // }
    }
}
