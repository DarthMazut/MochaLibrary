using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mocha.Dialogs
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
        /// <para> Not every implementation of <see cref="IDialogModule"/> can supports this event ! </para>
        /// </summary>
        event EventHandler Opened;

        /// <summary>
        /// Fires when dialog is about to close.
        /// <para> Not every implementation of <see cref="IDialogModule"/> can supports this event ! </para>
        /// </summary>
        event EventHandler<CancelEventArgs> Closing;

        /// <summary>
        /// Fires when dialog closes.
        /// </summary>
        event EventHandler Closed;

        /// <summary>
        /// Fires when dialog is done.
        /// <para>"Comrade soldier, you're done!"</para>
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
        /// Sets a * DataContext * for <see cref="View"/> object.
        /// </summary>
        /// <param name="dialog">Dialog logic to be set by * DataContext * mechanism.</param>
        void SetDataContext(IDialog dialog);

        /// <summary>
        /// Closes the dialog if open.
        /// </summary>
        void Close();
    }
}
