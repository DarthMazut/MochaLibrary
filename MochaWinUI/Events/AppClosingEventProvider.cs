using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using MochaCore.Events;
using MochaCore.Events.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace MochaWinUI.Events
{
    public class AppClosingEventProvider : BaseEventProvider<AppClosingEventArgs>
    {
        private readonly Window _mainWindow;
        private readonly AppWindow _appWindow;

        public AppClosingEventProvider(Window mainWindow)
        {
            _mainWindow = mainWindow;
            _appWindow = GetAppWindowFromWindow(mainWindow);
            _appWindow.Closing += OnClosing;
        }

        public override event EventHandler<AppClosingEventArgs>? Event;

        private async void OnClosing(AppWindow sender, AppWindowClosingEventArgs e)
        {
            // Execute synchronous events
            AppClosingEventArgs eventArgs = new AppClosingEventArgs(EventArgs.Empty);
            Event?.Invoke(sender, eventArgs);
            e.Cancel = eventArgs.Cancel;

            // Execute asynchronouse tasks
            if (AsyncInvocationList.Any())
            {
                e.Cancel = true;
                List<Task> parallelCollection = StartAndGetParallelCollection(eventArgs);

                await ExecuteAllNonParallel(eventArgs);
                await Task.WhenAll(parallelCollection);
                await Task.Yield();

                if (eventArgs.Cancel == false)
                {
                    _appWindow.Closing -= OnClosing;
                    _mainWindow.Close();
                }
            }
        }

        private AppWindow GetAppWindowFromWindow(Window window)
        {
            IntPtr hwnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
            WindowId winId = Win32Interop.GetWindowIdFromWindow(hwnd);
            return AppWindow.GetFromWindowId(winId);
        }
    }
}
