using System;
using System.Collections.Generic;
using System.Text;

namespace Mocha.Dialogs.Extensions.DI
{
    /// <summary>
    /// Provides an interface for <see cref="IDialogModule{T}"/> instantiation. Use it with Dependency Injection pattern.
    /// </summary>
    public interface IDialogFactory
    {
        /// <summary>
        /// Creates an instance of specified <see cref="IDialogModule{T}"/> type.
        /// </summary>
        /// <typeparam name="T">Type of internal <see cref="DialogControl"/> object.</typeparam>
        /// <param name="id">Specifies the dialog identifier.</param>
        IDialogModule<T> Create<T>(string id) where T : DialogControl;
    }
}
