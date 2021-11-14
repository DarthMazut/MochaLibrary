using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using MochaCore.Dialogs;
using MochaCore.Dialogs.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCoreWinUI.Dialogs
{
    /// <summary>
    /// Provides implementation of <see cref="IDialogModule"/> for WinUI <see cref="ContentDialog"/> and it's descendants.
    /// </summary>
    /// <typeparam name="TView">WinUI <see cref="ContentDialog"/> or descendant.</typeparam>
    /// <typeparam name="TControl">Type of <see cref="DialogControl"/>.</typeparam>
    public class ContentDialogModule<TView, TControl> : IDialogModule<TControl> where TView : ContentDialog where TControl : DialogControl
    {
        private Window _parentWindow;
        private TView _view;
        private IDialog<TControl> _dataContext;

        private bool _isOpen = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentDialogModule{TView, TControl}"/> class.
        /// </summary>
        /// <param name="parentWindow">Parent window for representing dialog.</param>
        /// <param name="contentDialog">Technology-specific view object.</param>
        public ContentDialogModule(Window parentWindow, TView contentDialog) : this(parentWindow, contentDialog, new SimpleDialogData<TControl>()) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentDialogModule{TView, TControl}"/> class.
        /// </summary>
        /// <param name="parentWindow">Parent window for representing dialog.</param>
        /// <param name="contentDialog">Technology-specific view object.</param>
        /// <param name="dataContext">An <see cref="IDialog{T}"/> object bounded to <see cref="View"/> instance by *DataBinding* mechanism.</param>
        public ContentDialogModule(Window parentWindow, TView contentDialog, IDialog<TControl> dataContext)
        {
            _view = contentDialog;
            SetDataContext(dataContext);
            
            // Workaround for bug https://github.com/microsoft/microsoft-ui-xaml/issues/2504
            _view.XamlRoot = parentWindow.Content.XamlRoot;
            _parentWindow = parentWindow;

            _view.Opened += (s, e) => OnOpened();
            _view.Closing += (s, e) => OnClosing(e);
            _view.Closed += (s, e) => OnClosed();
        }

        /// <inheritdoc/>
        public event EventHandler? Opening;

        /// <inheritdoc/>
        public event EventHandler? Opened;

        /// <inheritdoc/>
        public event EventHandler<CancelEventArgs>? Closing;

        /// <inheritdoc/>
        public event EventHandler? Closed;

        /// <inheritdoc/>
        public event EventHandler? Disposed;

        /// <inheritdoc/>
        public object View => _view;

        /// <inheritdoc/>
        public IDialog<TControl> DataContext => _dataContext;

        /// <inheritdoc/>
        IDialog IDialogModule.DataContext => _dataContext;

        /// <inheritdoc/>
        public bool IsOpen => _isOpen;

        /// <summary>
        /// A delegate which traverse visual tree in order to place a dialog as a child of specific visual object.
        /// </summary>
        public Action<Window, ContentDialog>? DialogPlacementDelegate { get; set; }

        /// <summary>
        /// Customizes view object based on properties within <see cref="DialogControl"/>.
        /// Use this delagate to avoiding subcalssing only for overriding <see cref="Customize(TView, TControl)"/>.
        /// <para>Setting this delegate overrides default <c>CustomizeCore()</c> implementation.</para>
        /// </summary>
        public Action<TView, TControl>? CustomizeDelegate { get; set; }

        /// <summary>
        /// Translates technology-specific dialog result object into technology-independent <see langword="bool?"/> value.
        /// Sets the dialog results within <see cref="DialogControl"/> object.
        /// Use this delagate to avoiding subcalssing only for overriding <see cref="HandleResult(ContentDialogResult, TControl)(TView, TControl)"/>.
        /// <para>Setting this delegate overrides default <c>HandleResultCore()</c> implementation.</para>
        /// </summary>
        public Func<ContentDialogResult, ContentDialogModule<TView, TControl>, TControl, bool?>? HandleResultDelegate { get; set; }

        /// <inheritdoc/>
        public void Close()
        {
            _view.Hide();
        }

        /// <summary>
        /// Perform cleaning operations allowing this object to be garbage collected.
        /// It's important to set *DataContext* for view object to <see langword="null"/> here.
        /// </summary>
        public virtual void Dispose()
        {
            _view.DataContext = null;
            _dataContext.DialogControl.Dispose();
            OnDisposeCore();
        }

        /// <inheritdoc/>
        public void SetDataContext(IDialog dialog)
        {
            if (dialog is not IDialog<TControl>)
            {
                throw new ArgumentException($"Data context for {GetType().Name} can only be of type {typeof(IDialog<TControl>)}");
            }

            _dataContext = (IDialog<TControl>)dialog;
            _view.DataContext = dialog;
            dialog.DialogControl.Activate(this);
        }

        /// <inheritdoc/>
        public async Task ShowAsync()
        {
            OnOpeningCore();
            Customize(_view, _dataContext.DialogControl);
            DialogPlacementDelegate?.Invoke(_parentWindow, _view);
            _isOpen = true;
            ContentDialogResult dialogResult = await _view.ShowAsync(ContentDialogPlacement.InPlace);
            _ = HandleResult(dialogResult, _dataContext.DialogControl);
        }

        /// <inheritdoc/>
        public async Task<bool?> ShowModalAsync()
        {
            OnOpeningCore();
            Customize(_view, _dataContext.DialogControl);
            _isOpen = true;
            ContentDialogResult dialogResult = await _view.ShowAsync();
            return HandleResult(dialogResult, _dataContext.DialogControl);
        }

        /// <summary>
        /// Content Dialog does not support synchronous Show() method.
        /// </summary>
        public void Show()
        {
            throw new InvalidOperationException("Content Dialog does not support synchronous Show() method.");
        }

        /// <summary>
        /// Content Dialog does not support synchronous ShowModal() method.
        /// </summary>
        public bool? ShowModal()
        {
            throw new InvalidOperationException("Content Dialog does not support synchronous ShowModal() method.");
        }

        /// <summary>
        /// Invokes an <see cref="Opening"/> event.
        /// </summary>
        protected virtual void OnOpeningCore() => Opening?.Invoke(this, EventArgs.Empty);

        /// <summary>
        /// Invokes an <see cref="Opened"/> event.
        /// </summary>
        protected virtual void OnOpenedCore() => Opened?.Invoke(this, EventArgs.Empty);

        /// <summary>
        /// Invokes an <see cref="Closing"/> event.
        /// </summary>
        protected virtual void OnClosingCore(CancelEventArgs e) => Closing?.Invoke(this, e);

        /// <summary>
        /// Invokes an <see cref="Closed"/> event.
        /// </summary>
        protected virtual void OnClosedCore() => Closed?.Invoke(this, EventArgs.Empty);

        /// <summary>
        /// Invokes an <see cref="Disposed"/> event.
        /// </summary>
        protected virtual void OnDisposeCore() => Disposed?.Invoke(this, EventArgs.Empty);

        private void OnOpened()
        {
            _isOpen = true;
            OnOpenedCore();
        }

        private void OnClosing(ContentDialogClosingEventArgs e)
        {
            CancelEventArgs args = new();
            OnClosingCore(args);
            if (args.Cancel)
            {
                e.Cancel = true;
            }
        }

        private void OnClosed()
        {
            _isOpen = false;
            OnClosedCore();
        }

        /// <summary>
        /// Customizes view object based on properties within <see cref="DialogControl"/>.
        /// </summary>
        /// <param name="view">View object to be customized.</param>
        /// <param name="dialogControl">A <see cref="DialogControl"/> object which serves as a source for customization.</param>
        protected virtual void CustomizeCore(TView view, TControl dialogControl)
        {
            string? title = dialogControl.Title;
            if (title is not null)
            {
                view.Title = title;
            }
        }

        /// <summary>
        /// Translates technology-specific dialog result object into technology-independent <see langword="bool?"/> value.
        /// Sets the dialog results within <see cref="DialogControl"/> object.
        /// </summary>
        /// <param name="result">Result to be translated.</param>
        /// <param name="dialogControl">Results should be set on this object.</param>
        protected virtual bool? HandleResultCore(ContentDialogResult result, TControl dialogControl)
        {
            dialogControl.DialogResult = result switch
            {
                ContentDialogResult.None => null,
                ContentDialogResult.Primary => true,
                ContentDialogResult.Secondary => false,
                _ => null,
            };

            return dialogControl.DialogResult;
        }

        private void Customize(TView view, TControl dialogControl)
        {
            if (CustomizeDelegate is not null)
            {
                CustomizeDelegate.Invoke(view, dialogControl);
            }
            else
            {
                CustomizeCore(view, dialogControl);
            }
        }

        private bool? HandleResult(ContentDialogResult result, TControl dialogControl)
        {
            if (HandleResultDelegate is not null)
            {
                return HandleResultDelegate.Invoke(result, this, dialogControl);
            }
            else
            {
                return HandleResultCore(result, dialogControl);
            }
        }
    }
}
