using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using MochaCore.Dispatching;
using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace MochaWinUI.Dispatching
{
    /// <summary>
    /// Provides an implementation of <see cref="IDispatcher"/> interface for WinUI 3 applications.
    /// </summary>
    public class WinUIDispatcher : IDispatcher
    {
        private readonly DispatcherQueue _dispatcherQueue;
        private readonly CoreDispatcher _dispatcher;

        /// <summary>
        /// Initializes a new instance of the <see cref="WinUIDispatcher"/> class.
        /// </summary>
        /// <param name="app">Root application object.</param>
        public WinUIDispatcher(Application app)
        {
            _dispatcherQueue = app.Resources.DispatcherQueue;
            _dispatcher = app.Resources.Dispatcher;
        }

        /// <inheritdoc/>
        public void EnqueueOnMainThread(Action action)
        {
            _dispatcherQueue.TryEnqueue(() => action.Invoke());
        }

        /// <inheritdoc/>
        public void RunOnMainThread(Action action)
        {
            SemaphoreSlim semaphore = new(1, 1);
            _dispatcherQueue.TryEnqueue(() => 
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
            _dispatcherQueue.TryEnqueue(async () => 
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
            if (_dispatcherQueue.HasThreadAccess && _dispatcher?.ShouldYield() != false)
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
