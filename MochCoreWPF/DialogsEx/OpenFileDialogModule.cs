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
    /// <summary>
    /// Provides a standard implementation of <see cref="IDialogModule{T}"/> for WPF <see cref="OpenFileDialog"/> class.
    /// </summary>
    public class OpenFileDialogModule : BrowseBaseDialogModule<OpenFileDialog, bool?, OpenFileDialogProperties>
    {
        public OpenFileDialogModule(Window mainWindow, OpenFileDialog view) : base(mainWindow, view)
        {
            Properties = new OpenFileDialogProperties();
        }

        protected override void ApplyPropertiesCore(OpenFileDialog dialog, OpenFileDialogProperties properties)
        {
            dialog.Title = properties.Title;
            dialog.Filter = properties.Filters.ToWpfFilterFormat();
            dialog.InitialDirectory = properties.InitialDirectory;
            dialog.Multiselect = properties.MultipleSelection;
        }

        protected override bool? HandleResultCore(OpenFileDialog dialog, bool? result, OpenFileDialogProperties properties)
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
        }

        protected override bool? ShowDialogCore(OpenFileDialog dialog, Window parent)
        {
            return dialog.ShowDialog(parent);
        }
    }
}
