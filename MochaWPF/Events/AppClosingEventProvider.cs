using Mocha.Events;
using Mocha.Events.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace MochaWPF.Events
{
    /// <summary>
    /// Provides WPF implementation for main window *OnClosing* event.
    /// </summary>
    public class AppClosingEventProvider : IEventProvider<AppClosingEventArgs>
    {
        private readonly Window _window;
        private readonly List<AsyncEventHandler<AppClosingEventArgs>> _asyncInvocationList = new List<AsyncEventHandler<AppClosingEventArgs>>();

        /// <inheritdoc/>
        public event EventHandler<AppClosingEventArgs> Event;

        /// <summary>
        /// Initializes a new instance of <see cref="AppClosingEventProvider"/> class.
        /// </summary>
        /// <param name="mainWindow">Main window of WPF application.</param>
        public AppClosingEventProvider(Window mainWindow)
        {
            _window = mainWindow;
            mainWindow.Closing += OnClosing;
        }

        /// <inheritdoc/>
        public void SubscribeAsync(AsyncEventHandler<AppClosingEventArgs> asyncEventHandler)
        {
            _asyncInvocationList.Add(asyncEventHandler);
        }

        /// <inheritdoc/>
        public void UnsubscribeAsync(Func<AppClosingEventArgs, IReadOnlyCollection<AsyncEventHandler>, Task> function)
        {
            int toRemove = _asyncInvocationList.FindIndex(h => h.Equals(function));
            _asyncInvocationList.RemoveAt(toRemove);
        }

        private async void OnClosing(object sender, CancelEventArgs e)
        {
            // Execute synchronous events
            AppClosingEventArgs eventArgs = new AppClosingEventArgs();
            Event?.Invoke(sender, eventArgs);
            e.Cancel = eventArgs.Cancel;

            // Execute asynchronouse tasks
            if (_asyncInvocationList.Any())
            {
                e.Cancel = true;

                List<Task> parallelCollection =
                    _asyncInvocationList
                    .Where(h => h.ExecuteInParallel)
                    .Select(h => h.Execute(eventArgs, _asyncInvocationList.AsReadOnly())).ToList();

                List<AsyncEventHandler<AppClosingEventArgs>> sortedInvocationList = 
                    _asyncInvocationList
                    .Where(h => h.ExecuteInParallel == false)
                    .OrderBy(h => h.Priority).ToList();

                foreach (AsyncEventHandler<AppClosingEventArgs> eventHandler in sortedInvocationList)
                {
                    await eventHandler.Execute(eventArgs, _asyncInvocationList.AsReadOnly());
                }

                await Task.WhenAll(parallelCollection);

                if (eventArgs.Cancel == false)
                {
                    _window.Closing -= OnClosing;
                    _window.Close();
                }
            }
        }
    }
}
