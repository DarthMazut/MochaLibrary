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
    /// Provides WPF implementation for <see cref="IEventProvider"/> class.
    /// </summary>
    public class EventProvider : IEventProvider
    {
        private readonly Window _window;
        private readonly List<Func<Task<AppClosingEventArgs>>> _onClosingTasks = new List<Func<Task<AppClosingEventArgs>>>();

        /// <inheritdoc/>
        public event EventHandler<AppClosingEventArgs> AppClosing;

        /// <inheritdoc/>
        public event EventHandler AppClosed;

        /// <summary>
        /// NOT APPLICABLE
        /// </summary>
        public event EventHandler<HardwareBackButtonPressedEventArgs> HardwareBackButtonPressed;

        /// <summary>
        /// Initializes a new instance of <see cref="EventProvider"/> class for WPF applications.
        /// </summary>
        /// <param name="mainWindow">Main application window.</param>
        public EventProvider(Window mainWindow)
        {
            _window = mainWindow;
            mainWindow.Closing += OnClosing;
            mainWindow.Closed += OnClosed;
        }

        public void SubscribeAsyncAppClosing(Func<Task<AppClosingEventArgs>> task)
        {
            _onClosingTasks.Add(task);
        }

        private void OnClosed(object sender, EventArgs e)
        {
            AppClosed?.Invoke(sender, e);
        }

        private async void OnClosing(object sender, CancelEventArgs e)
        {
            // Execute synchronous events
            AppClosingEventArgs eventArgs = new AppClosingEventArgs();
            AppClosing?.Invoke(sender, eventArgs);
            e.Cancel = eventArgs.Cancel;

            // Execute asynchronouse tasks
            if (_onClosingTasks.Any())
            { 
                e.Cancel = true;
                bool shouldClose = false;
                
                foreach (Func<Task<AppClosingEventArgs>> task in _onClosingTasks)
                {
                    shouldClose = !(await task.Invoke()).Cancel;
                }

                if (shouldClose)
                {
                    _window.Closing -= OnClosing;
                    _window.Close();
                }
            }
        }


    }
}
