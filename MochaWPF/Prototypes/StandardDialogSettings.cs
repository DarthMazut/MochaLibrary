using Mocha.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MochaWPF.Prototypes
{
    public class StandardDialogSettings
    {
        /// <summary>
        /// Maps <see cref="DialogParameters.PredefinedButtons"/> string into <see cref="MessageBoxButton"/> value.
        /// </summary>
        public Func<string, MessageBoxButton> ResolveButtons { get; set; }

        /// <summary>
        /// Maps <see cref="DialogParameters.Icon"/> string into <see cref="MessageBoxImage"/> value.
        /// </summary>
        public Func<string, MessageBoxImage> ResolveIcon { get; set; }

        /// <summary>
        /// Maps results from <see cref="MessageBoxResult"/> to nullable <see langword="bool"/> values.
        /// </summary>
        public Func<MessageBoxResult, bool?> ResolveResult { get; set; }

        /// <summary>
        /// Returns new instance of <see cref="StandardDialogSettings"/> class.
        /// </summary>
        public StandardDialogSettings()
        {
            ResolveButtons = ResolveButtonsDefault;
            ResolveIcon = ResolveIconDefault;
            ResolveResult = ResolveResultDefault;
        }

        private MessageBoxButton ResolveButtonsDefault(string predefinedButtons)
        {
            if (Enum.TryParse(predefinedButtons, out MessageBoxButton msgBoxButton))
            {
                return msgBoxButton;
            }

            return MessageBoxButton.OK;
        }

        private MessageBoxImage ResolveIconDefault(string icon)
        {
            if (Enum.TryParse(icon, out MessageBoxImage msgBoxImage))
            {
                return msgBoxImage;
            }

            return MessageBoxImage.None;
        }

        private bool? ResolveResultDefault(MessageBoxResult result)
        {
            switch (result)
            {
                case MessageBoxResult.None:
                    return null;
                case MessageBoxResult.OK:
                    return true;
                case MessageBoxResult.Cancel:
                    return false;
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
