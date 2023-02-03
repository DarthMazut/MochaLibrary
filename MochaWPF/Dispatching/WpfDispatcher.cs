using MochaCore.Dispatching;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace MochaWPF.Dispatching
{
    /// <summary>
    /// Provides an implementation of <see cref="IDispatcher"/> interface for WPF applications.
    /// </summary>
    public class WpfDispatcher : IDispatcher
    {
        readonly Dispatcher _dispatcher;

        /// <summary>
        /// Initializes a new instance of the <see cref="WpfDispatcher"/> class.
        /// </summary>
        /// <param name="app">WPF <see cref="Application"/> object.</param>
        public WpfDispatcher(Application app)
        {
            _dispatcher = app.Dispatcher;
        }

        /// <inheritdoc/>
        public void EnqueueOnMainThread(Action action)
        {
            _dispatcher.BeginInvoke(action);
        }

        /// <inheritdoc/>
        public void RunOnMainThread(Action action)
        {
            _dispatcher.Invoke(action);
        }

        /// <inheritdoc/>
        public async Task RunOnMainThreadAsync(Func<Task> func)
        {
            await _dispatcher.InvokeAsync(func);
        }

        /// <inheritdoc/>
        public async Task Yield()
        {
            if (_dispatcher.CheckAccess())
            {
                await Dispatcher.Yield();
            }
        }
    }
}
