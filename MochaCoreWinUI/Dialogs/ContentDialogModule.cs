using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using MochaCore.Dialogs;
using MochaCore.Dialogs.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCoreWinUI.Dialogs
{
    /// <summary>
    /// Provides implementation of <see cref="IDialogModule"/> for WinUI <see cref="ContentDialog"/> specifically.
    /// For objects that derives from <see cref="ContentDialog"/> use <see cref="GenericContentDialogModule{TView, TControl}"/>.
    /// </summary>
    public class ContentDialogModule : GenericContentDialogModule<ContentDialog, ContentDialogControl>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentDialogModule"/> class.
        /// </summary>
        /// <param name="parentWindow">Parent window for representing dialog.</param>
        public ContentDialogModule(Window parentWindow) :
            base(parentWindow, new ContentDialog(), new SimpleDialogData<ContentDialogControl>()) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentDialogModule"/> class.
        /// </summary>
        /// <param name="parentWindow">Parent window for representing dialog.</param>
        /// <param name="dataContext">An <see cref="IDialog{T}"/> object bounded to <see cref="View"/> instance by *DataBinding* mechanism.</param>
        public ContentDialogModule(Window parentWindow, IDialog<ContentDialogControl> dataContext) : 
            base(parentWindow, new ContentDialog(), dataContext) { }

        protected override void Customize(ContentDialog view, ContentDialogControl dialogControl)
        {
            if (CustomizeDelegate is null)
            {
                _view.Title = dialogControl.Title;
                _view.Content = dialogControl.Content;
                _view.PrimaryButtonText = dialogControl.PrimaryButtonText;
                _view.SecondaryButtonText = dialogControl.SecondaryButtonText;
                _view.CloseButtonText = dialogControl.CloseButtonText;
            }
            else
            {
                CustomizeDelegate.Invoke(view, dialogControl);
            }
        }

        #region SEALED MEMBERS

        public sealed override void Close() => base.Close();
        public sealed override void Dispose() => base.Dispose();
        public sealed override void SetDataContext(IDialog dialog) => base.SetDataContext(dialog);
        public sealed override Task ShowAsync() => base.ShowAsync();
        public sealed override Task<bool?> ShowModalAsync() => base.ShowModalAsync();
        protected sealed override void OnClosed() => base.OnClosed();
        protected sealed override void OnClosing(ContentDialogClosingEventArgs e) => base.OnClosing(e);
        protected sealed override void OnOpened() => base.OnOpened();

        #endregion
    }
}
