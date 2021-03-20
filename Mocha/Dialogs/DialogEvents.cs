using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Mocha.Dialogs
{
    /// <summary>
    /// Contains a delegates which can be set by <see cref="IDialog"/> implementation.
    /// Thos delegates are then invoked by <see cref="IDialogModule"/> at proper time.
    /// </summary>
    public class DialogEvents : IDisposable
    {
        private IDialogModule _module;
        private IDialog _dialog;

        /// <summary>
        /// Fires when dialog is about to close.
        /// <para> Not every implementation of <see cref="IDialogModule"/> supports this event ! </para>
        /// </summary>
        public event EventHandler<CancelEventArgs> Closing;

        /// <summary>
        /// Returns new instance of <see cref="DialogEvents"/> class.
        /// </summary>
        /// <param name="dialog"><see cref="IDialog"/> implementation associated with this object.</param>
        public DialogEvents(IDialog dialog)
        {
            _dialog = dialog;
        }

        /// <summary>
        /// Perform cleaning operations allowing this object to be garbage collected.
        /// </summary>
        public void Dispose()
        {
            UnsubscribeAll();
        }

        internal void ActivateSubscribtion()
        {
            _module = FindCorrespondingModule(_dialog);
            SubscribeAll();
        }

        private IDialogModule FindCorrespondingModule(IDialog dialog)
        {
            foreach (IDialogModule module in DialogManager.GetActiveDialogs())
            {
                if(module.DataContext == dialog)
                {
                    return module;
                }  
            }

            return null;
        }

        private void SubscribeAll()
        {
            if(_module != null)
            {
                _module.Closing += OnClosing;
            }
        }

        private void UnsubscribeAll()
        {
            if(_module != null)
            {
                _module.Closing -= OnClosing;
            }
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            Closing?.Invoke(this, e);
        }
    }
}
