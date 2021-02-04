using System;
using System.Collections.Generic;
using System.Text;

namespace Mocha.Dialogs
{
    /// <summary>
    /// Abstraction for an object which provides a concrete implementations of the <see cref="IDialogModule"/>. 
    /// </summary>
    public interface IDialogFactory
    {
        /// <summary>
        /// Returns a concrete implementation of <see cref="IDialogModule"/> object.
        /// </summary>
        /// <param name="parameters">
        /// Serves for configuration of a process of creating concrete <see cref="IDialogModule"/> implementation.
        /// </param>
        IDialogModule Create(string[] parameters);
    }
}
