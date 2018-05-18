using System;
using System.Threading;
using System.Threading.Tasks;

namespace SharpSettings
{
    public interface ISettingsWatcher<TId, TSettings> : IDisposable where TSettings : WatchableSettings<TId>
    {
        Task<TSettings> GetSettingsAsync(CancellationToken token);
    }
}
