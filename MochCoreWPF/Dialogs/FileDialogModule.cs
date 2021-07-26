using System;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using MochaCore.Dialogs;
using MochaCore.Dialogs.Extensions;

namespace MochaCoreWPF.Dialogs
{
    /// <summary>
    /// Provides a standard implementation of a <see cref="IDialogModule"/> for WPF
    /// <see cref="FileDialog"/> classes. Use this class for handling Open and Save 
    /// dialogs from Win32 namespace.
    /// </summary>
    public sealed class FileDialogModule : BaseDialogModule<FileDialogControl>
    {
        private readonly FileDialog _view;

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
        /// <param name="dataContext">Provides a backend data for represented <see cref="FileDialog"/>.</param>
        public FileDialogModule(Application application, FileDialog dialog, IDialog? dataContext) : base(application, dataContext)
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

        /// <inheritdoc/>
        protected override bool? ShowModalCore()
        {
            return _view.ShowDialog(GetParentWindow());
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        protected override void CloseCore()
        {
            throw new NotSupportedException("Closing File Dialog is currently not supported in MochaLib :(");
        }

        /// <inheritdoc/>
        protected override void DisposeCore() { }

        /// <inheritdoc/>
        protected override void SetDataContextCore(IDialog dataContext) { }

        /// <inheritdoc/>
        protected override void SetResults(bool? result)
        {
            base.SetResults(result);

            if (result == true)
            {
                DataContext.DialogControl.SelectedPath = _view.FileName;
            }
        }

        /// <inheritdoc/>
        protected override void Customize(DialogControl dialogControl)
        {
            _view.Title = DataContext.DialogControl.Title;
            _view.Filter = DataContext.DialogControl.Filter;
            _view.DefaultExt = DataContext.DialogControl.DefaultExtension;
            _view.InitialDirectory = DataContext.DialogControl.InitialDirectory;
        }
    }
}
