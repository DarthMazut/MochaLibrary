using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.DialogsEx
{
    /// <summary>
    /// Implement together with <see cref="IDataContextDialog{T}"/> or its descendant to allow for code execution
    /// on <see cref="IDialogModule"/> initialization and disposal.
    /// </summary>
    public interface IDialogInitialize
    {
        /// <summary>
        /// Called during construction of associated <see cref="IDialogModule"/>. Use this method to register to
        /// any events exposed by <see cref="IDataContextDialog{T}.DialogModule"/>.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Called when associated <see cref="IDialogModule"/> is being disposed. Use this method to unsubscribe from events 
        /// registered within <see cref="Initialize"/> method and/or free any resources used by implementing
        /// dialog view model.
        /// </summary>
        void Uninitialize();
    }
}
