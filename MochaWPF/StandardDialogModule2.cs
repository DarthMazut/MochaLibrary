using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Mocha.Dialogs;

namespace MochaWPF
{
    public class StandardDialogModule2 : BaseDialogModule
    {
        /// <summary>
        /// There is no view object which represents WPF <see cref="MessageBox"/> dialog
        /// therefore this always returns <see langword="null"/>.
        /// </summary>
        public override object View => null;

        /// <summary>
        /// Default construtcor. Uses build-in simple implementaion of <see cref="IDialog"/>.
        /// </summary>
        public StandardDialogModule2(Application application) : this(application, null) { }

        /// <summary>
        /// Returns new instance of <see cref="StandardDialogModule"/> class.
        /// </summary>
        /// <param name="dialogData">Backend for represented <see cref="MessageBox"/> dialog.</param>
        public StandardDialogModule2(Application application, IDialog dialogData) : base(application)
        {
            if(dialogData is null)
            {
                SetDataContext(new SimpleDialogData());
            }
            else
            {
                SetDataContext(dialogData);
            }
            
        }

        /// <summary>
        /// Performs closing action.
        /// </summary>
        protected override void CloseCore()
        {
            throw new NotSupportedException("Closing StandardDialogModule is currently not supported in MochaLib :(");
        }

        /// <summary>
        /// Performs cleaning operations. It is important to set 
        /// *DataContex* of view object to <see langword="null"/> here.
        /// </summary>
        protected override void DisposeCore()
        {
            // StandardDialogModule does not have anything to dispose.
        }

        /// <summary>
        /// Sets provided <see cref="IDialog"/> data as a *DataContext* for
        /// view object.
        /// </summary>
        /// <param name="dialog">Backend for represented dialog.</param>
        protected override void SetDataContextCore(IDialog dialog)
        {
            // No further DataContext action needs to be perfoormed.
        }

        /// <summary>
        /// Show dialog in modal mode here.
        /// </summary>
        protected override bool? ShowModalCore()
        {
            bool? result = HandleDialogDisplay(null, DataContext.DialogParameters);

            OnClosed();
            DataContext.DialogResult = result;

            return result;
        }

        /// <summary>
        /// Show dialog in modal mode in asynchronous manner here.
        /// </summary>
        protected override Task<bool?> ShowModalCoreAsync()
        {
            return Task.Run(() =>
            {
                bool? result = ShowModalCore();
                return result;
            });
        }

        /// <summary>
        /// Handles dialog display. Uses <see cref="DialogParameters"/> object to customize
        /// associated dialog.
        /// </summary>
        /// <param name="parent">Parent window.</param>
        /// <param name="parameters">Parameters for dialog customization.</param>
        protected bool? HandleDialogDisplay(Window parent, DialogParameters parameters)
        {
            if (parameters == null) throw new ArgumentNullException("parameters was null.");

            string title = parameters.Title ?? string.Empty;
            string content = parameters.Message ?? string.Empty;
            MessageBoxButton buttons = MessageBoxButton.OK;
            MessageBoxImage icon = MessageBoxImage.None;

            if (Enum.TryParse(parameters.PredefinedButtons, out MessageBoxButton msgBoxButton))
            {
                buttons = msgBoxButton;
            }

            if (Enum.TryParse(parameters.Icon, out MessageBoxImage msgBoxImage))
            {
                icon = msgBoxImage;
            }

            if (parent != null)
            {
                return MessageBoxResultToBoolean(MessageBox.Show(parent, content, title, buttons, icon));
            }
            else
            {
                return MessageBoxResultToBoolean(MessageBox.Show(content, title, buttons, icon));
            }
        }

        /// <summary>
        /// Maps given <see cref="MessageBoxResult"/> value to nullable <see langword="bool"/> object.
        /// </summary>
        /// <param name="messageBoxResult">Value to be mapped.</param>
        protected virtual bool? MessageBoxResultToBoolean(MessageBoxResult messageBoxResult)
        {
            switch (messageBoxResult)
            {
                case MessageBoxResult.None:
                    return null;
                case MessageBoxResult.OK:
                    return true;
                case MessageBoxResult.Cancel:
                    return null;
                case MessageBoxResult.Yes:
                    return true;
                case MessageBoxResult.No:
                    return false;
                default:
                    return null;
            }
        }
    }
}
