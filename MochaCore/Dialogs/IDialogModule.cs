using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace MochaCore.Dialogs
{
    /// <summary>
    /// Exposes methods and properties for interaction with technology-independent dialog representation.
    /// </summary>
    public interface IDialogModule : IDisposable
    {
        /// <summary>
        /// Reference to technology-specific view object.
        /// </summary>
        object View { get; }

        /// <summary>
        /// An <see cref="IDialog"/> object bounded to <see cref="View"/>
        /// instance by *DataBinding* mechanism.
        /// </summary>
        IDialog DataContext { get; }

        /// <summary>
        /// Specifies whether this dialog is currently open.
        /// </summary>
        bool IsOpen { get; }

        ///// <summary>
        ///// Determines whether this instance should be automatically disposed after closing.
        ///// </summary>
        //bool DisposeOnClose { get; set; }

        /// <summary>
        /// Fires when dialog is about to be displayed.
        /// </summary>
        event EventHandler Opening;

        /// <summary>
        /// Fires right after the dialog opens.
        /// </summary>
        event EventHandler Opened;

        /// <summary>
        /// Fires when dialog is about to close.
        /// </summary>
        event EventHandler<CancelEventArgs> Closing;

        /// <summary>
        /// Fires when dialog closes.
        /// </summary>
        event EventHandler Closed;

        /// <summary>
        /// Fires when dialog is done.
        /// <para><c>"Comrade soldier, you're done!"</c></para>
        /// </summary>
        event EventHandler Disposed;

        /// <summary>
        /// Opens a dialog represented by this instance.
        /// </summary>
        void Show();

        /// <summary>
        /// Opens a dialog represented by this instance in modal mode. 
        /// Returns result of dialog interaction.
        /// </summary>
        bool? ShowModal();

        /// <summary>
        /// Opens a dialog represented by this instance in asynchronous manner.
        /// </summary>
        Task ShowAsync();

        /// <summary>
        /// Asynchronously opens a dialog represented by this instance in modal mode. 
        /// Returns result of dialog interaction.
        /// </summary>
        Task<bool?> ShowModalAsync();

        /// <summary>
        /// Sets a DataContext for <see cref="View"/> object.
        /// </summary>
        /// <param name="dialog">Dialog logic to be set by DataContext mechanism.</param>
        void SetDataContext(IDialog dialog);

        /// <summary>
        /// Closes the dialog if open.
        /// </summary>
        void Close();
    }

    /// <summary>
    /// Exposes methods and properties for interaction with technology-independent dialog representation.
    /// </summary>
    /// <typeparam name="T">Specifies statically typed parameters for the associated dialog logic.</typeparam>
    public interface IDialogModule<T> : IDialogModule where T : DialogControl
    {
        /// <summary>
        /// An <see cref="IDialog"/> object bounded to <see cref="IDialogModule.View"/>
        /// instance by DataBinding mechanism.
        /// </summary>
        new IDialog<T> DataContext { get; }
    }
}
