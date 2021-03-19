﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Mocha.Dialogs;

namespace MochaWPF.Prototypes
{
    public class StandardDialogModulePrototype : BaseDialogModule
    {
        public sealed override object View => null;

        public StandardDialogModulePrototype(Application application, IDialog dataContext) : base(application, dataContext)
        {
        }

        protected sealed override bool? ShowModalCore()
        {
            Window parent = GetParentWindow();
            string title = DataContext.DialogParameters.Title;
            string message = DataContext.DialogParameters.Message;
            string buttons = DataContext.DialogParameters.PredefinedButtons;
            string icon = DataContext.DialogParameters.Icon;

            return ResolveResult(MessageBox.Show(parent, message, title, ResolveButtons(buttons), ResolveIcon(icon)));
        }

        protected sealed override Task<bool?> ShowModalCoreAsync()
        {
            return Task.Run(() =>
            {
                return ShowModalCore();
            });
        }

        protected sealed override void CloseCore()
        {
            throw new NotSupportedException("Closing StandardDialogModule is currently not supported in MochaLib");
        }

        protected sealed override void DisposeCore() { }

        protected sealed override void SetDataContextCore(IDialog dataContext) { }

        /// <summary>
        /// Maps <see cref="DialogParameters.PredefinedButtons"/> string into <see cref="MessageBoxButton"/> value.
        /// </summary>
        /// <param name="predefinedButtons">Value to be mapped.</param>
        protected virtual MessageBoxButton ResolveButtons(string predefinedButtons)
        {
            if (Enum.TryParse(predefinedButtons, out MessageBoxButton msgBoxButton))
            {
                return msgBoxButton;
            }

            return MessageBoxButton.OK;
        }

        /// <summary>
        /// Maps <see cref="DialogParameters.Icon"/> string into <see cref="MessageBoxImage"/> value.
        /// </summary>
        /// <param name="icon">Value to be mapped.</param>
        protected virtual MessageBoxImage ResolveIcon(string icon)
        {
            if (Enum.TryParse(icon, out MessageBoxImage msgBoxImage))
            {
                return msgBoxImage;
            }

            return MessageBoxImage.None;
        }

        /// <summary>
        /// Maps results from <see cref="MessageBoxResult"/> to nullable <see langword="bool"/> values.
        /// </summary>
        /// <param name="result">Value to be mapped.</param>
        protected virtual bool? ResolveResult(MessageBoxResult result)
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

        #region SEALED MEMBERS

        protected sealed override void Customize(DialogParameters dialogParameters) => base.Customize(dialogParameters);
        protected sealed override Window GetParentWindow() => base.GetParentWindow();
        protected sealed override void SetBehaviors() => base.SetBehaviors();
        public sealed override void SetDataContext(IDialog dialog) => base.SetDataContext(dialog);
        protected sealed override void SetParent(Window parentWindow) => base.SetParent(parentWindow);
        public sealed override void Show() => base.Show();
        public sealed override Task ShowAsync() => base.ShowAsync();
        protected sealed override void SetResults(bool? result) => base.SetResults(result);
        protected sealed override void ShowCore() => base.ShowCore();
        protected sealed override Task ShowCoreAsync() => base.ShowCoreAsync();
        public sealed override bool? ShowModal() => base.ShowModal();
        public sealed override Task<bool?> ShowModalAsync() => base.ShowModalAsync();

        #endregion
    }
}
