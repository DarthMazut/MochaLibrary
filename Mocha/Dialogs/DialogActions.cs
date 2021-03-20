using System;
using System.Collections.Generic;
using System.Text;

namespace Mocha.Dialogs
{
    /// <summary>
    /// Contains available actions for associated dialog.
    /// </summary>
    public class DialogActions
    {
        private readonly Action _closeAction;

        /// <summary>
        /// Creates new instance of <see cref="DialogActions"/> class.
        /// </summary>
        /// <param name="module">Module which actions are exposed.</param>
        public DialogActions(IDialogModule module)
        {
            _closeAction = module.Close;
        }

        /// <summary>
        /// Closes the dialog if open.
        /// </summary>
        public void Close()
        {
            _closeAction.Invoke();
        }
    }
}
