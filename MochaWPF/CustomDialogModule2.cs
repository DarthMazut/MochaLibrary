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
    public class CustomDialogModule2 : BaseDialogModule
    {
        private readonly Window _view;

        /// <summary>
        /// Return view object associated with represented dialog.
        /// </summary>
        public override sealed object View => _view;

        /// <summary>
        /// Returns a new instance of <see cref="CustomDialogModule"/> class.
        /// </summary>
        /// <param name="application">A reference to WPF <see cref="Application"/> object.</param>
        /// <param name="window">A <see cref="Window"/> which will be associated with created <see cref="CustomDialogModule"/>.</param>
        /// <param name="dataContext">A default dialog logic bounded to <see cref="CustomDialogModule"/> by *DataContext* mechanism.</param>
        public CustomDialogModule2(Application application, Window window, IDialog dataContext) : base(application)
        {
            _view = window;
            SetDataContext(dataContext);

            window.Closed += (s, e) => OnClosed();
            window.Loaded += (s, e) => IsOpen = true;
        }

        protected override void ShowCore()
        {
            _view.Owner = GetParentWindow();
            _view.Show();
        }

        protected override Task ShowCoreAsync()
        {
            return Task.Run(() =>
            {
                Application.Dispatcher.Invoke(() =>
                {
                    ShowCore();
                });
            });
        }

        protected override bool? ShowModalCore()
        {
            _view.Owner = GetParentWindow();
            bool? result = _view.ShowDialog();
            DataContext.DialogResult = result;
            return result;
        }

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

        protected override void CloseCore()
        {
            _view.Close();
        }

        protected override void DisposeCore()
        {
            _view.DataContext = null;
        }

        protected override void SetDataContextCore(IDialog dialog)
        {
            _view.DataContext = dialog;
        }
    }
}
