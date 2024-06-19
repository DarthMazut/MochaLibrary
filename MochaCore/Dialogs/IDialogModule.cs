using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.Dialogs
{
    /// <summary>
    /// Provides base type for <see cref="IDialogModule{T}"/> and its descendants.
    /// </summary>
    public interface IDialogModule : IDisposable
    {
        // TODO: Why View is nullable? Are there any cases that module does not
        // contain technology-specyfic dialog object?

        /// <summary>
        /// Technology-specific dialog object which is represented by this module.
        /// </summary>
        object? View { get; }

        /// <summary>
        /// Asynchronously opens a dialog represented by this instance in modal mode. 
        /// Returns result of dialog interaction.
        /// </summary>
        /// <param name="host">
        /// The object which hosts this dialog while being shown.
        /// <para>You should pass '<see langword="this"/>' here.</para>
        /// </param>
        Task<bool?> ShowModalAsync(object? host);

        /// <summary>
        /// Fires whenever dialog is about to be opened.
        /// </summary>
        event EventHandler? Opening;

        /// <summary>
        /// Fires whenever dialog closes.
        /// </summary>
        event EventHandler? Closed;

        /// <summary>
        /// Fires when dialog is done.
        /// <para><c>"Comrade soldier, you're done!"</c></para>
        /// </summary>
        event EventHandler? Disposed;
    }

    /// <summary>
    /// Exposes methods and properties for interaction with technology-independent dialog representation.
    /// </summary>
    /// <typeparam name="T">Specifies statically typed properties for the associated dialog.</typeparam>
    public interface IDialogModule<T> : IDialogModule where T : new()
    {
        /// <summary>
        /// Statically typed properties object which serves for configuration of this module.
        /// </summary>
        T Properties { get; set; }
    }
}
