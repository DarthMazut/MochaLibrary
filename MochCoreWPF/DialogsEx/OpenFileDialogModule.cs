using Microsoft.Win32;
using MochaCore.DialogsEx;
using MochaCore.DialogsEx.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MochCoreWPF.DialogsEx
{
    public class OpenFileDialogModule : IDialogModule<OpenFileDialogProperties>
    {
        private readonly Window _mainWindow;
        private readonly OpenFileDialog _dialog;
        private Window? _parent;

        public OpenFileDialogModule(Window mainWindow, OpenFileDialog dialog)
        {
            _mainWindow = mainWindow;
            _dialog = dialog;

            FindParent = (host) => ParentResolver.FindParent<OpenFileDialogProperties>(host) ?? _mainWindow;
        }

        public object? View => _dialog;

        public object? Parent => _parent;

        public OpenFileDialogProperties Properties { get; set; } = new();

        public Action<OpenFileDialog, OpenFileDialogProperties> ApplyProperties { get; set; } = (dialog, properties) =>
        {
            dialog.Title = properties.Title;
            dialog.Filter = properties.Filters.ToWpfFilterFormat();
            dialog.InitialDirectory = properties.InitialDirectory;
            dialog.Multiselect = properties.MultipleSelection;
        };

        public Func<OpenFileDialog, bool?, OpenFileDialogProperties, bool?> HandleResult { get; set; } = (dialog, result, properties) =>
        {
            if (dialog.Multiselect)
            {
                foreach (string selectedFile in dialog.FileNames)
                {
                    properties.SelectedPaths.Add(selectedFile);
                }
            }
            else
            {
                properties.SelectedPaths.Add(dialog.FileName);
            }
            

            return result;
        };

        public Func<object, Window> FindParent { get; set; }

        public event EventHandler? Opening;
        public event EventHandler? Closed;
        public event EventHandler? Disposed;

        public void Dispose()
        {
            Disposed?.Invoke(this, EventArgs.Empty);
        }

        public Task<bool?> ShowModalAsync(object host)
        {
            ApplyProperties.Invoke(_dialog, Properties);
            Opening?.Invoke(this, EventArgs.Empty);
            _parent = FindParent(host);
            bool? result = HandleResult.Invoke(_dialog, _dialog.ShowDialog(_parent), Properties);
            _parent = null;
            Closed?.Invoke(this, EventArgs.Empty);
            return Task.FromResult(result);
        }
    }
}
