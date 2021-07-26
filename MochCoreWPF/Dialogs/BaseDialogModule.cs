using MochaCore.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

namespace MochaCoreWPF.Dialogs
{
    /// <summary>
    /// Provides a base class for implementation of <see cref="IDialogModule"/> for WPF apps.
    /// </summary>
    public abstract class BaseDialogModule : IDialogModule
    {
        private bool _isOpen;
        
        /// <summary>
        /// A reference to WPF <see cref="System.Windows.Application"/> object.
        /// </summary>
        protected Application Application { get; set; }

        /// <summary>
        /// Returns a reference to underlying <see cref="Window"/> object.
        /// </summary>
        public abstract object? View { get; }

        /// <inheritdoc/>
        public IDialog DataContext { get; protected set; }

        /// <summary>
        /// Determines whether dialog has been disposed.
        /// </summary>
        protected bool IsDisposed { get; set; }

        /// <inheritdoc/>
        public bool IsOpen
        {
            get => _isOpen;
            protected set
            {
                if (value != _isOpen)
                {
                    if (value == true)
                    {
                        _isOpen = true;
                        Opening?.Invoke(this, EventArgs.Empty);
                    }
                    else
                    {
                        _isOpen = false;
                        Closed?.Invoke(this, EventArgs.Empty);
                    }
                }
            }
        }

        /// <inheritdoc/>
        public event EventHandler? Opening;

        /// <inheritdoc/>
        public abstract event EventHandler? Opened;

        /// <inheritdoc/>
        public abstract event EventHandler<CancelEventArgs>? Closing;

        /// <inheritdoc/>
        public event EventHandler? Closed;

        /// <summary>
        /// Fires when dialog is disposed and DataContext on <see cref="View"/> object is cleared.
        /// </summary>
        public event EventHandler? Disposed;

        /// <summary>
        /// Returns a new instance of <see cref="BaseDialogModule"/> class.
        /// </summary>
        /// <param name="application">A reference to WPF <see cref="Application"/> object.</param>
        /// <param name="dataContext">A default dialog logic bounded to <see cref="BaseDialogModule"/> by *DataContext* mechanism.</param>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public BaseDialogModule(Application application, IDialog? dataContext)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            Application = application ?? throw new ArgumentNullException(nameof(application));
            SetDataContext(dataContext ?? new SimpleDialogData());
        }

        /// <summary>
        /// Closes the dialog if open.
        /// </summary>
        public void Close()
        {
            if (IsOpen)
            {
                CloseCore();
                IsOpen = false;
            }
        }

        /// <inheritdoc/>
        public virtual void SetDataContext(IDialog dialog)
        {
            DataContext = dialog;
            dialog.DialogControl.Activate(this);
        }

        /// <summary>
        /// Handles the process of displaying the dialog in non-modal manner.
        /// </summary>
        public virtual void Show()
        {
            if (IsOpen)
            {
                throw new InvalidOperationException($"{View?.GetType() ?? GetType()} was already opened");
            }

            if (IsDisposed)
            {
                throw new InvalidOperationException("Cannot show already disposed dialog");
            }

            SetDataContextCore(DataContext);
            SetParent(GetParentWindow());
            Customize(DataContext.DialogControl);
            IsOpen = true;
            ShowCore();
        }

        /// <summary>
        /// Handles the process of displaying the dialog asynchronously in non-modal manner.
        /// </summary>
        public virtual Task ShowAsync()
        {
            if (IsOpen)
            {
                throw new InvalidOperationException($"{View?.GetType() ?? GetType()} was already opened");
            }

            if (IsDisposed)
            {
                throw new InvalidOperationException("Cannot show already disposed dialog");
            }

            SetDataContextCore(DataContext);
            SetParent(GetParentWindow());
            Customize(DataContext.DialogControl);
            IsOpen = true;
            return ShowCoreAsync();
        }

        /// <summary>
        /// Handles the process of displaying the dialog in modal manner.
        /// </summary>
        public virtual bool? ShowModal()
        {
            if (IsOpen)
            {
                throw new InvalidOperationException($"{View?.GetType() ?? GetType()} was already opened");
            }

            if (IsDisposed)
            {
                throw new InvalidOperationException("Cannot show already disposed dialog");
            }

            SetDataContextCore(DataContext);
            SetParent(GetParentWindow());
            Customize(DataContext.DialogControl);
            IsOpen = true;
            bool? result = ShowModalCore();
            SetResults(result);
            IsOpen = false;

            return result;
        }

        /// <summary>
        /// Handles the process of displaying the dialog asynchronously in modal manner.
        /// </summary>
        public virtual async Task<bool?> ShowModalAsync()
        {
            if (IsOpen)
            {
                throw new InvalidOperationException($"{View?.GetType() ?? GetType()} was already opened");
            }

            if (IsDisposed)
            {
                throw new InvalidOperationException("Cannot show already disposed dialog");
            }

            SetDataContextCore(DataContext);
            SetParent(GetParentWindow());
            Customize(DataContext.DialogControl);
            IsOpen = true;
            bool? result = await ShowModalCoreAsync();
            SetResults(result);
            IsOpen = false;

            return result;
        }

        /// <summary>
        /// Perform cleaning operations allowing this object to be garbage collected.
        /// </summary>
        public void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }
            else
            {
                Close();
                DataContext.DialogControl.Dispose();
                DisposeCore();
                IsDisposed = true;
                Disposed?.Invoke(this, EventArgs.Empty);
            }

            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Sets DataContext on <see cref="View"/> object.
        /// </summary>
        /// <param name="dataContext">Object to be set as data context.</param>
        protected abstract void SetDataContextCore(IDialog dataContext);

        /// <summary>
        /// Uses <see cref="IDialog.DialogControl"/> to customize current dialog view instance.
        /// </summary>
        /// <param name="dialogControl">Parameters which serves for current dialog customization.</param>
        protected virtual void Customize(DialogControl dialogControl) { }

        /// <summary>
        /// Sets parent <see cref="Window"/> for current dialog view object. 
        /// </summary>
        /// <param name="parentWindow">Window resolved as parent.</param>
        protected virtual void SetParent(Window? parentWindow) { }

        /// <summary>
        /// Sets the results of dialog interaction within <see cref="IDialog.DialogControl"/>.
        /// </summary>
        /// <param name="result">Result of dialog interaction.</param>
        protected virtual void SetResults(bool? result)
        {
            DataContext.DialogControl.DialogResult = result;
        }

        /// <summary>
        /// Opens dialog view object in non-modal manner.
        /// </summary>
        protected virtual void ShowCore()
        {
            throw new InvalidOperationException(GetNotSupportedMsg());
        }

        /// <summary>
        /// Opens dialog view object in modal mode.
        /// </summary>
        protected virtual bool? ShowModalCore()
        {
            throw new InvalidOperationException(GetNotSupportedMsg());
        }

        /// <summary>
        /// Asynchronously opens dialog view object in non-modal manner.
        /// </summary>
        protected virtual Task ShowCoreAsync()
        {
            throw new InvalidOperationException(GetNotSupportedMsg());
        }

        /// <summary>
        /// Asynchronously opens dialog view object in modal mode.
        /// </summary>
        protected virtual Task<bool?> ShowModalCoreAsync()
        {
            throw new InvalidOperationException(GetNotSupportedMsg());
        }

        /// <summary>
        /// Performs disposing operation. It's important to set DataContext to <see langword="null"/> here.
        /// </summary>
        protected abstract void DisposeCore();

        /// <summary>
        /// Performs closing operation on view object or throws
        /// <see cref="InvalidOperationException"/> if such not available.
        /// </summary>
        protected abstract void CloseCore();

        /// <summary>
        /// Returns parent <see cref="Window"/> based on value from <see cref="DialogControl.Parent"/>.
        /// </summary>
        protected virtual Window? GetParentWindow()
        {
            Window? parent = null;

            Application?.Dispatcher.Invoke(() =>
            {
                List<IDialogModule> modules = DialogManager.GetActiveDialogs();
                IDialog? parentDialog = DataContext.DialogControl.Parent;

                IDialogModule? parentModule = modules.Where(m => m.DataContext == parentDialog).FirstOrDefault();

                foreach (Window window in Application.Windows)
                {
                    if (window == parentModule?.View)
                    {
                        parent = window;
                        return;
                    }
                }

                parent = Application.Windows[0];
            });

            return parent;
        }

        private string GetNotSupportedMsg()
        {
            string currentMember = MethodBase.GetCurrentMethod()!.Name;
            return $"{currentMember} method was not defined. To use this method please override {currentMember} when deriving from {GetType()} class.";
        }
    }

    /// <summary>
    /// Provides a base class for implementation of <see cref="IDialogModule"/> for WPF apps.
    /// </summary>
    /// <typeparam name="T">Specifies statically typed parameters for the associated dialog logic.</typeparam>
    public abstract class BaseDialogModule<T> : BaseDialogModule, IDialogModule<T> where T : DialogControl
    {
        /// <summary>
        /// An <see cref="IDialog"/> object bounded to <see cref="BaseDialogModule.View"/>
        /// instance by *DataBinding* mechanism.
        /// </summary>
        new public IDialog<T> DataContext => (base.DataContext as IDialog<T>)!;

        /// <summary>
        /// Returns a new instance of <see cref="BaseDialogModule{T}"/> class.
        /// </summary>
        /// <param name="application">A reference to WPF <see cref="Application"/> object.</param>
        /// <param name="dataContext">A default dialog logic bounded to <see cref="BaseDialogModule"/> by DataContext mechanism.</param>
        protected BaseDialogModule(Application application, IDialog? dataContext) : base(application, dataContext) 
        {
            SetDataContext(dataContext ?? new SimpleDialogData<T>());
        }

       
    }
}
