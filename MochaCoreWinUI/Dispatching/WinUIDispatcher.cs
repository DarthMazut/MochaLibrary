using Microsoft.UI.Xaml;
using MochaCore.Dispatching;
using System;
using System.Threading.Tasks;

namespace MochaCoreWinUI.Dispatching
{
    public class WinUIDispatcher : IDispatcher
    {
        private readonly Window _mainWindow;

        public WinUIDispatcher(Window mainWindow)
        {
            _mainWindow = mainWindow;
        }

        public void EnqueueOnMainThread(Action action)
        {
            _mainWindow.DispatcherQueue.TryEnqueue(() => action.Invoke());
        }

        public void RunOnMainThread(Action action)
        {
            throw new NotImplementedException();
        }

        public Task RunOnMainThreadAsync(Func<Task> func)
        {
            throw new NotImplementedException();
        }

        public async Task Yield()
        {
            await Task.Yield();
        }
    }
}
