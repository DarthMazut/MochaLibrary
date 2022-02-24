using Microsoft.Win32;
using MochaCore.DialogsEx.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MochCoreWPF.DialogsEx
{
    public class SaveFileDialogModule : BrowseBaseDialogModule<SaveFileDialog, bool?, SaveFileDialogProperties>
    {
        public SaveFileDialogModule(Window mainWindow, SaveFileDialog view) : base(mainWindow, view)
        {
        }

        public override SaveFileDialogProperties Properties { get; set; } = new();

        public override Action<SaveFileDialog, SaveFileDialogProperties> ApplyProperties { get; set; } = (dialog, properties) =>
        {
            dialog.Title = properties.Title;
            dialog.Filter = properties.Filters.ToWpfFilterFormat();
        };

        public override Func<SaveFileDialog, Window, bool?> ShowModalCore { get; set; } = (dialog, parent) =>
        {
            return dialog.ShowDialog(parent);
        };

        public override Func<SaveFileDialog, bool?, SaveFileDialogProperties, bool?> HandleResult { get; set; } = (dialog, result, properties) =>
        {
            throw new NotImplementedException();
        };
    }
}
