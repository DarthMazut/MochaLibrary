using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.NavigationEx
{
    public class ModalNavigationData
    {
        public ModalNavigationData(string originId, TaskCompletionSource<object?> modalNavigationCompletionSource)
        {
            OriginId = originId ?? throw new ArgumentNullException(nameof(originId));
            CompletionSource = modalNavigationCompletionSource ?? throw new ArgumentNullException(nameof(modalNavigationCompletionSource));
        }

        public string OriginId { get; }

        public TaskCompletionSource<object?> CompletionSource { get; }
    }
}
