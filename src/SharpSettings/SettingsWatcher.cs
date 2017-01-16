using System;
using System.Threading;
using System.Threading.Tasks;

namespace SharpSettings
{
    public abstract class SettingsWatcher<TSettings, TId> where TSettings : WatchableSettings<TId>
    {
        private readonly IDataStore<TSettings, TId> _store;
        private readonly CancellationTokenSource _cts;
        private TSettings _settings;
        private Task _watcherTask;
        private bool _stopWatcher;

        public TId SettingsId { get; protected set; }
        public TSettings Settings => _settings;


        public SettingsWatcher(IDataStore<TSettings, TId> settingsStore, TId settingsId)
        {
            _store = settingsStore;
            _cts = new CancellationTokenSource();
            _stopWatcher = false;
            _watcherTask = Task.Factory.StartNew(RunWatchAsync, _cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
            SettingsId = settingsId;
            var count = 0;
            while(Settings == null && count < 10)
            {
                Task.Delay(1000).Wait();
                count++;
            }

            if (Settings == null) throw new Exception("Failed to load settings.");
        }

        public void StopWatch()
        {
            _stopWatcher = true;
            var watcherStopped = _watcherTask.Wait(10000);
            if (watcherStopped) return;

            _cts.Cancel(true);
            watcherStopped = _watcherTask.Wait(10000);
            if (watcherStopped == false)
                throw new Exception("Failed to stop SettingsWatcher.");
        }

        private async Task RunWatchAsync()
        {
            while (!_stopWatcher)
            {
                _settings = await _store.FindAsync(SettingsId);
                if(_settings == null)
                    throw new Exception("Settings not found.");
                Console.WriteLine("Settings updated.");
                await Task.Delay(1000);
            }
        }
    }
}
