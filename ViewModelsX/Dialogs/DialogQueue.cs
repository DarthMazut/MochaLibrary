using MochaCore.Dialogs;
using MochaCore.Dispatching;
using MochaCore.Windowing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModelsX.Dialogs
{
    public class DialogQueue : IDisposable
    {
        private readonly object _host;
        private readonly Queue<ModuleWithHandler> _queue = new();

        public DialogQueue(object host)
        {
            _host = host;
        }

        public void EnqueueDialog(IDialogModule dialogModule) => EnqueueDialog(dialogModule, null);

        public void EnqueueDialog(IDialogModule dialogModule, Action<bool?>? resultHandler)
        {
            if (_queue.Any())
            {
                _queue.Enqueue(new ModuleWithHandler(dialogModule, resultHandler));
            }
            else
            {
                dialogModule.Closed += EnqueuedModuleClosed;
                _queue.Enqueue(new ModuleWithHandler(dialogModule, resultHandler));
                DispatcherManager.GetMainThreadDispatcher().EnqueueOnMainThread(async () =>
                {
                    bool? result = await dialogModule.ShowModalAsync(_host);
                    resultHandler?.Invoke(result);
                });
            }
        }

        public void Dispose()
        {
            if (_queue.Any())
            {
                _queue.First().Module.Closed -= EnqueuedModuleClosed;
            }

            foreach (ModuleWithHandler moduleWithHandler in _queue)
            {
                moduleWithHandler.Module.Dispose();
            }
        }

        private async void EnqueuedModuleClosed(object? sender, EventArgs e)
        {
            if (sender is IDialogModule module)
            {
                _queue.Dequeue();
                module.Closed -= EnqueuedModuleClosed;
                if (_queue.Any())
                {
                    ModuleWithHandler moduleWithHandler = _queue.First();
                    moduleWithHandler.Module.Closed += EnqueuedModuleClosed;
                    bool? result = await moduleWithHandler.Module.ShowModalAsync(_host);
                    moduleWithHandler.ResultHandler?.Invoke(result);
                }
            }
        }

        private record ModuleWithHandler(IDialogModule Module, Action<bool?>? ResultHandler);
    }
}
