using Microsoft.UI.Xaml;
using MochaCore.Dispatching;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MochaCoreWinUI.Dispatching
{
    /// <summary>
    /// Provides an implementation of <see cref="IDispatcher"/> interface for WinUI 3 applications.
    /// </summary>
    public class WinUIDispatcher : IDispatcher
    {
        private readonly Window _mainWindow;

        /// <summary>
        /// Initializes a new instance of the <see cref="WinUIDispatcher"/> class.
        /// </summary>
        /// <param name="mainWindow">Application's main window.</param>
        public WinUIDispatcher(Window mainWindow)
        {
            _mainWindow = mainWindow;
        }

        /// <inheritdoc/>
        public void EnqueueOnMainThread(Action action)
        {
            _mainWindow.DispatcherQueue.TryEnqueue(() => action.Invoke());
        }

        /// <inheritdoc/>
        public void RunOnMainThread(Action action)
        {
            SemaphoreSlim semaphore = new(1, 1);
            _mainWindow.DispatcherQueue.TryEnqueue(() => 
            {
                action.Invoke();
                semaphore.Release();
            });

            semaphore.Wait();
        }

        /// <inheritdoc/>
        public async Task RunOnMainThreadAsync(Func<Task> func)
        {
            CancellationTokenSource cts = new();
            _mainWindow.DispatcherQueue.TryEnqueue(async () => 
            {
                await func.Invoke();
                cts.Cancel();
            });

            try
            {
                await Task.Delay(Timeout.Infinite, cts.Token);
            }
            catch (OperationCanceledException) { }
            finally 
            {
                cts.Dispose();
            }
        }

        /// <inheritdoc/>
        public async Task Yield()
        {
            if (_mainWindow.DispatcherQueue.HasThreadAccess && _mainWindow.Dispatcher?.ShouldYield() != false)
            {
                await Task.Factory.StartNew(
                    () => { },
                    CancellationToken.None,
                    TaskCreationOptions.None,
                    SynchronizationContext.Current != null ? TaskScheduler.FromCurrentSynchronizationContext() : TaskScheduler.Current);
            }
        }
    }
}
