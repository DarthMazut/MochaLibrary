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
            Properties = new SaveFileDialogProperties();
        }

        protected override void ApplyPropertiesCore(SaveFileDialog dialog, SaveFileDialogProperties properties)
        {
            dialog.Title = properties.Title;
            dialog.Filter = properties.Filters.ToWpfFilterFormat();
        }

        protected override bool? HandleResultCore(SaveFileDialog dialog, bool? result, SaveFileDialogProperties properties)
        {
            properties.SelectedPath = dialog.FileName;
            return result;
        }

        protected override bool? ShowDialogCore(SaveFileDialog dialog, Window parent)
        {
            return dialog.ShowDialog(parent);
        }
    }
}
