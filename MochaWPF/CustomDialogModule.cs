using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Mocha.Dialogs;

namespace MochaWPF
{
    /// <summary>
    /// Provides a typical implementation of <see cref="IDialogModule"/> for WPF apps.
    /// </summary>
    public class CustomDialogModule : BaseDialogModule
    {
        protected Window _view;

        /// <summary>
        /// Returns a reference to underlying <see cref="Window"/> object.
        /// </summary>
        public sealed override object View => _view;

        /// <summary>
        /// Returns a new instance of <see cref="CustomDialogModule"/> class.
        /// </summary>
        /// <param name="application">A reference to WPF <see cref="Application"/> object.</param>
        /// <param name="window">A <see cref="Window"/> which will be associated with created <see cref="CustomDialogModule"/>.</param>
        /// <param name="dataContext">A default dialog logic bounded to <see cref="CustomDialogModule"/> by *DataContext* mechanism.</param>
        public CustomDialogModule(Application application, Window window, IDialog dataContext) : base(application, dataContext)
        {
            _view = window;

            _view.Loaded += (s, e) => IsOpen = true;
            _view.Closed += (s, e) => IsOpen = false;
            _view.Closing += (s, e) => OnClosing(e);
            _view.Loaded += (s, e) => OnOpened();
        }

        /// <summary>
        /// Performs "Show" operation on associated view object.
        /// </summary>
        protected sealed override void ShowCore()
        {
            _view.Show();
        }

        /// <summary>
        /// Performs "ShowDialog" operation on associated view object.
        /// </summary>
        protected sealed override bool? ShowModalCore()
        {
            return _view.ShowDialog();
        }

        /// <summary>
        /// Performs "Show" operation on associated view object in asynchronous manner.
        /// </summary>
        protected sealed override Task ShowCoreAsync()
        {
            return Task.Run(() => 
            {
                Application.Dispatcher.Invoke(() =>
                {
                    ShowCore();
                });
            });
        }

        /// <summary>
        /// Performs "ShowDialog" operation on associated view object in asynchronous manner.
        /// </summary>
        protected sealed override Task<bool?> ShowModalCoreAsync()
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
        /// Performs "Close" operation on associated view object.
        /// </summary>
        protected sealed override void CloseCore()
        {
            _view.Close();
        }

        /// <summary>
        /// Performs cleaning operations.
        /// </summary>
        protected override void DisposeCore()
        {
            _view.DataContext = null;
        }

        /// <summary>
        /// Sets the *DataContext* for associated dialog view object.
        /// </summary>
        /// <param name="dataContext">Object to be set as *DataContext*.</param>
        protected sealed override void SetDataContextCore(IDialog dataContext)
        {
            _view.DataContext = dataContext;
        }

        /// <summary>
        /// Sets the parent <see cref="Window"/> for associated dialog view object.
        /// </summary>
        /// <param name="parentWindow">Window resolved as parent.</param>
        protected sealed override void SetParent(Window parentWindow)
        {
            _view.Owner = parentWindow;
        }

        #region SEALED MEMBERS

        /// <summary>
        /// Returns parent <see cref="Window"/> based on value from <see cref="DialogParameters"/>.
        /// </summary>
        protected sealed override Window GetParentWindow()
        {
            return base.GetParentWindow();
        }

        /// <summary>
        /// Sets a * DataContext * for <see cref="View"/> object.
        /// </summary>
        /// <param name="dialog">Dialog logic to be set by * DataContext * mechanism.</param>
        public sealed override void SetDataContext(IDialog dialog)
        {
            base.SetDataContext(dialog);
        }

        #endregion
    }
}
