using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.Navigation
{
    internal class NavigationStackItem : INavigationStackItem
    {
        private TaskCompletionSource<object?>? _tsc;

        public NavigationStackItem(INavigationLifecycleModule module)
        {
            Module = module ?? throw new ArgumentNullException(nameof(module));
        }

        INavigationModule INavigationStackItem.Module => Module;

        public INavigationLifecycleModule Module { get; }

        public bool IsModalOrigin => _tsc is not null;

        public void SetModal()
        {
            _tsc = new TaskCompletionSource<object?>();
        }

        public void SetResult(object? result)
        {
            _tsc!.SetResult(result);
        }

        public async Task<object?> PopResultAsync()
        {
            object? result = await _tsc!.Task;
            _tsc = null;
            return result;
        }
    }
}
