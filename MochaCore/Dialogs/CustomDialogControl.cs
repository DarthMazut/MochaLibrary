using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.Dialogs
{
    /*
    /// <summary>
    /// Exposes API for interaction with <see cref="ICustomDialogModule{T}"/>.
    /// </summary>
    /// <typeparam name="TProperties">The type of the dialog properties object associated with related dialog module.</typeparam>
    public class CustomDialogControl<TProperties> : DataContextDialogControl<TProperties> where TProperties : DialogProperties, new ()
    {
        private ICustomDialogModule<TProperties> _typedModule;

        /// <summary>
        /// Returns the related <see cref="ICustomDialogModule{TProperties}"/> object.
        /// </summary>
        new public ICustomDialogModule<TProperties> Module
        {
            get
            {
                InitializationGuard();
                return _typedModule;
            }
        }

        /// <summary>
        /// Should be called only once by <see cref="ICustomDialogModule{T}"/> related to this instance.
        /// Throws <see cref="InvalidOperationException"/> if this instance has been already initialized.
        /// Throws <see cref="ArgumentException"/> if provided module is not <see cref="ICustomDialogModule{T}"/>.
        /// </summary>
        /// <param name="dialogModule"><see cref="ICustomDialogModule{T}"/> which is related to this instance.</param>
        public override void Initialize(IDataContextDialogModule<TProperties> dialogModule)
        {
            if (dialogModule is not ICustomDialogModule<TProperties>)
            {
                throw new ArgumentException($"{GetType().Name} can only be initialized with {nameof(ICustomDialogModule<DialogProperties>)}.");
            }

            base.Initialize(dialogModule);
            _typedModule = (ICustomDialogModule<TProperties>)dialogModule;
        }

        /// <summary>
        /// Subscribes to the <see cref="IDialogOpened.Opened"/> event of the related <see cref="ICustomDialogModule{T}"/>.
        /// </summary>
        /// <param name="subscribingFunction">The delegate to be executed when the dialog is opened.</param>
        public void SubscribeOnDialogOpened(Action subscribingFunction)
        {
            _onDialogOpenedDelegate = subscribingFunction;
        }

        /// <summary>
        /// Subscribes to the <see cref="IDialogOpened.Opened"/> event of the related <see cref="ICustomDialogModule{T}"/>.
        /// </summary>
        /// <param name="subscribingFunction">The asynchronous delegate to be executed when the dialog is opened.</param>
        public void SubscribeOnDialogOpened(Func<Task> subscribingFunction)
        {
            _onDialogOpenedAsyncDelegate = subscribingFunction;
        }

        /// <summary>
        /// Subscribes to the <see cref="IDialogClosing.Closing"/> event of the related <see cref="ICustomDialogModule{T}"/>.
        /// </summary>
        /// <param name="subscribingFunction">The delegate to be executed when the dialog is closing.</param>
        public void SubscribeOnDialogClosing(Action subscribingFunction)
        {
            _onDialogClosingDelegate = subscribingFunction;
        }

        /// <summary>
        /// Subscribes to the <see cref="IDialogClosing.Closing"/> event of the related <see cref="ICustomDialogModule{T}"/>.
        /// </summary>
        /// <param name="subscribingFunction">The asynchronous delegate to be executed when the dialog is closing.</param>
        public void SubscribeOnDialogClosing(Func<Task> subscribingFunction)
        {
            _onDialogClosingAsyncDelegate = subscribingFunction;
        }

        /// <summary>
        /// Closes the related <see cref="ICustomDialogModule{T}"/> if it is currently open. 
        /// Throws an <see cref="InvalidOperationException"/> if DialogControl hasn't been initialized at the time this
        /// method was invoked.
        /// </summary>
        /// <param name="result">Determines the result of dialog interaction.</param>
        public void Close(bool? result)
        {
            InitializationGuard();
            _typedModule.Close(result);
        }
    }
    */
}
