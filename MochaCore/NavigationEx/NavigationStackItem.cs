using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.NavigationEx
{
    public class NavigationStackItem : INavigationStackItem
    {
        public NavigationStackItem(INavigationLifecycleModule module) : this(module, null) { }

        public NavigationStackItem(INavigationLifecycleModule module, TaskCompletionSource<object?>? modalCompletionSource)
        {
            Module = module ?? throw new ArgumentNullException(nameof(module));
            ModalCompletionSource = modalCompletionSource;
        }

        INavigationModule INavigationStackItem.Module => Module;

        public INavigationLifecycleModule Module { get; }

        public TaskCompletionSource<object?>? ModalCompletionSource { get; set; }

        public bool IsModalOrigin => ModalCompletionSource is not null;     
    }
}
