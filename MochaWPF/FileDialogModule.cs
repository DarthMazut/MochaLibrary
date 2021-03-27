using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using Mocha.Dialogs;

namespace MochaWPF
{
    /// <summary>
    /// Provides a standard implementation of a <see cref="IDialogModule"/> for WPF
    /// <see cref="FileDialog"/> classes. Use this class for handling Open and Save 
    /// dialogs from Win32 namespace.
    /// </summary>
    public sealed class FileDialogModule : BaseDialogModule
    {
        private FileDialog _view;

        /// <summary>
        /// Returns the underlying <see cref="FileDialog"/> concrete implementation. 
        /// </summary>
        public override object View => _view;

        /// <summary>
        /// Returns new instance of <see cref="FileDialogModule"/>.
        /// </summary>
        /// <param name="application">A reference to WPF <see cref="Application"/> object.</param>
        /// <param name="dialog">A concrete implementation of <see cref="FileDialog"/> abstract class.</param>
        public FileDialogModule(Application application, FileDialog dialog) : this(application, dialog, null) { }

        /// <summary>
        /// Returns new instance of <see cref="FileDialogModule"/>.
        /// </summary>
        /// <param name="application">A reference to WPF <see cref="Application"/> object.</param>
        /// <param name="dialog">A concrete implementation of <see cref="FileDialog"/> abstract class.</param>
        /// <param name="dataContext">Provides a backedn data for represented <see cref="FileDialog"/>.</param>
        public FileDialogModule(Application application, FileDialog dialog, IDialog dataContext) : base(application, dataContext)
        {
            _view = dialog;
            _view.FileOk += (s, e) => OnClosing(e);
        }

        /// <summary>
        /// This method is not supported for <see cref="FileDialog"/> implementation.
        /// </summary>
        protected override void ShowCore()
        {
            throw new NotSupportedException("File Dialog Module can only be displayed as modal.");
        }

        /// <summary>
        /// This method is not supported for <see cref="FileDialog"/> implementation.
        /// </summary>
        protected override Task ShowCoreAsync()
        {
            throw new NotSupportedException("File Dialog Module can only be displayed as modal.");
        }

        /// <summary>
        /// Opens dialog view object in modal mode.
        /// </summary>
        protected override bool? ShowModalCore()
        {
            return _view.ShowDialog(GetParentWindow());
        }

        /// <summary>
        /// Asynchronously opens dialog view object in modal mode.
        /// </summary>
        protected override Task<bool?> ShowModalCoreAsync()
        {
            return Task.Run(() =>
            {
                bool? result = null;

                Application.Dispatcher.Invoke(() =>
                {
                    result = ShowModalCore();
                });

                return result;
            });
        }

        /// <summary>
        /// Performs closing operation on view object or throws
        /// <see cref="InvalidOperationException"/> if such not available.
        /// </summary>
        protected override void CloseCore()
        {
            throw new NotSupportedException("Closing File Dialog is currently not supported in MochaLib :(");
        }

        /// <summary>
        /// Performs disposing operation. It's important to set *DataContext* to <see langword="null"/> here.
        /// </summary>
        protected override void DisposeCore() { }

        /// <summary>
        /// Sets *DataContext* on view object.
        /// </summary>
        /// <param name="dataContext">Object to be set as data context.</param>
        protected override void SetDataContextCore(IDialog dataContext) { }

        /// <summary>
        /// Sets the results of dialog interaction within <see cref="IDialog.DialogControl"/> and 
        /// <see cref="IDialog.DialogControl"/> if necessary.
        /// </summary>
        /// <param name="result">Result of dialog interaction.</param>
        protected override void SetResults(bool? result)
        {
            base.SetResults(result);

            if (result == true)
            {
                DataContext.DialogControl.DialogValue = _view.FileName;
            }
        }

        /// <summary>
        /// Uses <see cref="IDialog.DialogControl"/> to customize current dialog view instance.
        /// </summary>
        /// <param name="control">Parameters which serves for current dialog customization.</param>
        protected override void Customize(DialogControl dialogControl)
        {
            _view.Title = DataContext.DialogControl.Title;
            _view.Filter = DataContext.DialogControl.Filter;
            _view.DefaultExt = DataContext.DialogControl.DefaultExtension;
            _view.InitialDirectory = DataContext.DialogControl.InitialDirectory;
        }
    }
}
