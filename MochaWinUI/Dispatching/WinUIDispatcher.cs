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

        /// <inheritdoc/>
        public bool HasThreadAccess => _dispatcherQueue.HasThreadAccess;

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
        public void RunOnMainThread(Action action)
        {
            if (_dispatcherQueue.HasThreadAccess)
            {
                action.Invoke();
                return;
            }

            SemaphoreSlim semaphore = new(1, 1);
            semaphore.Wait();
            _dispatcherQueue.TryEnqueue(() => 
            {
                action.Invoke();
                semaphore.Release();
            });

            semaphore.Wait();
            semaphore.Dispose();
        }

        /// <inheritdoc/>
        public void RunOnMainThread(Func<Task> asyncAction)
        {
            if (_dispatcherQueue.HasThreadAccess)
            {
                throw new InvalidOperationException(
                    $"You called {nameof(RunOnMainThread)} with async delegate on UI thread. " +
                    $"Remember that {nameof(RunOnMainThread)} blocks the current thread until async delegate is completed. " +
                    "You cannot simultaneously block UI thread and enqueue delegate to await something on UI thread because that leads to deadlock.");
            }

            SemaphoreSlim semaphore = new(1, 1);
            semaphore.Wait();
            _dispatcherQueue.TryEnqueue(async () =>
            {
                await asyncAction.Invoke();
                semaphore.Release();
            });

            semaphore.Wait();
            semaphore.Dispose();
        }

        /// <inheritdoc/>
        public Task RunOnMainThreadAsync(Action action)
        {
            TaskCompletionSource tsc = new();
            _dispatcherQueue.TryEnqueue(() =>
            {
                action.Invoke();
                tsc.SetResult();
            });

            return tsc.Task;
        }

        /// <inheritdoc/>
        public Task RunOnMainThreadAsync(Func<Task> asyncAction)
        {
            TaskCompletionSource tsc = new();
            _dispatcherQueue.TryEnqueue(async () =>
            {
                await asyncAction.Invoke();
                tsc.SetResult();
            });

            return tsc.Task;
        }

        /// <inheritdoc/>
        public void EnqueueOnMainThread(Action action)
        {
            _dispatcherQueue.TryEnqueue(() => action.Invoke());
        }

        /// <inheritdoc/>
        public void EnqueueOnMainThread(Func<Task> asyncAction)
        {
            _dispatcherQueue.TryEnqueue(async () => await asyncAction.Invoke());
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
