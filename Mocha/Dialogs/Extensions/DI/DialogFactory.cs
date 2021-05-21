using System;
using System.Collections.Generic;
using System.Text;

namespace Mocha.Dialogs.Extensions.DI
{
    /// <summary>
    /// Provides an implementation of <see cref="IDialogFactory"/> interface.
    /// </summary>
    public class DialogFactory : IDialogFactory
    {
        /// <inheritdoc/>
        public IDialogModule<T> Create<T>(string id) where T : DialogControl
        {
            return (IDialogModule<T>)DialogManager.GetDialog(id);
        }
    }
}
