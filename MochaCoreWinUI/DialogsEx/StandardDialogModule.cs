using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using MochaCore.DialogsEx;
using MochaCore.DialogsEx.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCoreWinUI.DialogsEx
{
    public class StandardDialogModule : IDialogModule<MessageDialogProperties>
    {
        private readonly ContentDialog _view;
        private object? _parent;

        public StandardDialogModule(Window parentWindow, ContentDialog view)
        {
            _view = view;

            // Workaround for bug https://github.com/microsoft/microsoft-ui-xaml/issues/2504
            _view.XamlRoot = parentWindow.Content.XamlRoot;
        }

        public MessageDialogProperties Properties { get; set; } = new();
        public object? View { get; }

        public object? Parent { get; }

        public event EventHandler? Opening;

        public event EventHandler? Closed;

        public event EventHandler? Disposed;

        public async Task<bool?> ShowModalAsync(object host)
        {
            CustomizeDialog();
            return HandleResult(await _view.ShowAsync());
        }

        private void CustomizeDialog()
        {
            _view.Title = Properties.Title;
            _view.Content = Properties.Message;
        }

        private bool? HandleResult(ContentDialogResult result)
        {
            return result == ContentDialogResult.Primary ? true : false;
        }
    }
}
