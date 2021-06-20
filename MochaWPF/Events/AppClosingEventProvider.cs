using Mocha.Events;
using Mocha.Events.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
    public class AppClosingEventProvider : BaseEventProvider<AppClosingEventArgs>
    {
        private readonly Window _window;
        private bool _allowConstructorSubscribtion;
        private EventHandler<AppClosingEventArgs> _event;

        /// <inheritdoc/>
        public override event EventHandler<AppClosingEventArgs> Event
        {
            add
            {
                ThrowIfInConstructor();
                _event += value; 
            }
            remove { _event -= value; }
        }

        /// <summary>
        /// Initializes a new instance of <see cref="AppClosingEventProvider"/> class.
        /// </summary>
        /// <param name="mainWindow">Main window of WPF application.</param>
        public AppClosingEventProvider(Window mainWindow) : this(mainWindow, false) { }

        /// <summary>
        /// Initializes a new instance of <see cref="AppClosingEventProvider"/> class.
        /// </summary>
        /// <param name="mainWindow">Main window of WPF application.</param>
        /// <param name="allowConstructorSubscribtion">Determines whether an exception will be thrown when trying to subscribe inside constructors.</param>
        public AppClosingEventProvider(Window mainWindow, bool allowConstructorSubscribtion)
        {
            _window = mainWindow;
            _allowConstructorSubscribtion = allowConstructorSubscribtion;
            mainWindow.Closing += OnClosing;
        }

        /// <inheritdoc/>
        public override void SubscribeAsync(AsyncEventHandler<AppClosingEventArgs> asyncEventHandler)
        {
            ThrowIfInConstructor();
            base.SubscribeAsync(asyncEventHandler);
        }

        private async void OnClosing(object sender, CancelEventArgs e)
        {
            // Execute synchronous events
            AppClosingEventArgs eventArgs = new AppClosingEventArgs(e);
            _event?.Invoke(sender, eventArgs);
            e.Cancel = eventArgs.Cancel;

            // Execute asynchronouse tasks
            if (AsyncInvocationList.Any())
            {
                e.Cancel = true;
                List<Task> parallelCollection = StartAndGetParallelCollection(eventArgs);
                List<AsyncEventHandler<AppClosingEventArgs>> sortedInvocationList = GetSortedInvocationList();

                foreach (AsyncEventHandler<AppClosingEventArgs> eventHandler in sortedInvocationList)
                {
                    await eventHandler.Execute(eventArgs, AsyncInvocationList.AsReadOnly());
                }

                await Task.WhenAll(parallelCollection);

                await Dispatcher.Yield();
                if (eventArgs.Cancel == false)
                {
                    _window.Closing -= OnClosing;
                    _window.Close();
                }
            }
        }

        private void ThrowIfInConstructor()
        {
            if (!_allowConstructorSubscribtion && new StackFrame(2).GetMethod().IsConstructor)
            {
                throw new Exception("Do not subscribe to events within constructor method. For INavigatable elements use IOnNavigatedTo(Async) and IOnNavigatingFrom(Async) methods.");
            }
        }
    }
}
