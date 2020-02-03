using System;
using System.Threading;
using System.Threading.Tasks;

namespace SharpSettings
{
    #if NETSTANDARD2_1
    public interface ISettingsWatcher<TId, TSettings> : IDisposable, IAsyncDisposable where TSettings : WatchableSettings<TId>
    #else
    public interface ISettingsWatcher<TId, TSettings> : IDisposable where TSettings : WatchableSettings<TId>
    #endif
    {
        /// <summary>
        /// Get the <see cref="TSettings"/> object
        /// </summary>
        /// <returns>The <see cref="TSettings"/> object if available, otherwise null</returns>
        TSettings GetSettings();

        /// <summary>
        /// Wait for the <see cref="TSettings"/> object to become available and get it
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        ValueTask<TSettings> GetSettingsAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Restart the <see cref="ISettingsWatcher{TId, TSettings}"/>. Safe to use in both faulted and unfaulted states.
        /// Blocks until startup has completed.
        /// </summary>
        /// <param name="timeout">The timeout in milliseconds, set to -1 for no timeout</param>
        /// <returns>A <see cref="bool"/> value indicating if the restart was successful within the <paramref name="timeout"/>.</returns>
        bool Restart(int timeout);

        /// <summary>
        /// Asynchronously restart the <see cref="ISettingsWatcher{TId, TSettings}"/>. Safe to use in both faulted and unfaulted states.
        /// Blocks until startup has completed.
        /// </summary>
        /// <param name="timeout">The timeout in milliseconds, set to -1 for no timeout</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to cancel the operation</param>
        /// <returns>A <see cref="bool"/> value indicating if the restart was successful within the <paramref name="timeout"/>.</returns>
        ValueTask<bool> RestartAsync(int timeout, CancellationToken cancellationToken = default);

        /// <summary>
        /// Synchronously wait for the <see cref="ISettingsWatcher{TId, TSettings}"/> to startup.
        /// Since the <see cref="ISettingsWatcher{TId, TSettings}"/> may require network I/O and significant work
        /// to provide the settings, it may require some amount of time to startup. If applications must block
        /// until this infrastructure is setup, call this method.
        /// </summary>
        /// <param name="timeout">The timeout in milliseconds, set to 0 for no timeout</param>
        /// <returns>A <see cref="bool"/> value indicating if the startup was successful within the <paramref name="timeout"/>.</returns>
        bool WaitForStartup(int timeout);

        /// <summary>
        /// Wait for the <see cref="ISettingsWatcher{TId, TSettings}"/> to startup.
        /// Since the <see cref="ISettingsWatcher{TId, TSettings}"/> may require network I/O and significant work
        /// to provide the settings, it may require some amount of time to startup. If applications must block
        /// until this infrastructure is setup, <see langword="await"/> this method.
        /// </summary>
        /// <param name="timeout">The timeout in milliseconds, set to 0 for no timeout</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to cancel the operation</param>
        /// <returns>A <see cref="bool"/> value indicating if the startup was successful within the <paramref name="timeout"/>.</returns>
        ValueTask<bool> WaitForStartupAsync(int timeout, CancellationToken cancellationToken = default);

        /// <summary>
        /// Synchronously wait for the <see cref="ISettingsWatcher{TId, TSettings}"/> to startup.
        /// Since the <see cref="ISettingsWatcher{TId, TSettings}"/> may require network I/O and significant work
        /// to provide the settings, it may require some amount of time to startup. If applications must block
        /// until this infrastructure is setup, call this method.
        /// </summary>
        /// <param name="timeout">The amount of time to wait for startup, set to <see cref="TimeSpan.Zero"/> for no timeout</param>
        /// <returns>A <see cref="bool"/> value indicating if the startup was successful within the <paramref name="timeout"/>.</returns>
        bool WaitForStartup(TimeSpan timeout);
        
        /// <summary>
        /// Wait for the <see cref="ISettingsWatcher{TId, TSettings}"/> to startup.
        /// Since the <see cref="ISettingsWatcher{TId, TSettings}"/> may require network I/O and significant work
        /// to provide the settings, it may require some amount of time to startup. If applications must block
        /// until this infrastructure is setup, <see langword="await"/> this method.
        /// </summary>
        /// <param name="timeout">The amount of time to wait for startup, set to <see cref="TimeSpan.Zero"/> for no timeout</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to cancel the operation</param>
        /// <returns>A <see cref="bool"/> value indicating if the startup was successful within the <paramref name="timeout"/>.</returns>
        ValueTask<bool> WaitForStartupAsync(TimeSpan timeout, CancellationToken cancellationToken = default);

        /// <summary>
        /// A value indicating if an internal fault has stopped the <see cref="ISettingsWatcher{TId, TSettings}"/>
        /// </summary>
        /// <returns>A <see cref="bool"/> value indicating the fault state.</returns>
        bool IsFaulted();

        /// <summary>
        /// A value indicating if the <see cref="ISettingsWatcher{TId, TSettings}"/> is is running
        /// </summary>
        /// <returns>A <see cref="bool"/> value indicating the internal run state.</returns>
        bool IsRunning();
    }
}
