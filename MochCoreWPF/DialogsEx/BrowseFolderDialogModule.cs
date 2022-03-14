using MochaCore.DialogsEx.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using Application = System.Windows.Application;

namespace MochCoreWPF.DialogsEx
{
    public class BrowseFolderDialogModule : BrowseBaseDialogModule<FolderBrowserDialog, DialogResult, BrowseFolderDialogProperties>
    {
        public BrowseFolderDialogModule(Application application) : this(application.MainWindow, new FolderBrowserDialog()) { }

        public BrowseFolderDialogModule(Window mainWindow, FolderBrowserDialog dialog) : base(mainWindow, dialog)
        {
            Properties = new BrowseFolderDialogProperties();
        }

        protected override void ApplyPropertiesCore(FolderBrowserDialog dialog, BrowseFolderDialogProperties properties)
        {
            dialog.RootFolder = Environment.SpecialFolder.Desktop;
            dialog.SelectedPath = properties.InitialDirectory;
            dialog.UseDescriptionForTitle = true;
            dialog.Description = properties.Title;
        }

        protected override bool? HandleResultCore(FolderBrowserDialog dialog, DialogResult result, BrowseFolderDialogProperties properties)
        {
            if (result == DialogResult.OK || result == DialogResult.Yes)
            {
                properties.SelectedPath = dialog.SelectedPath;
                return true;
            }

            if (result == DialogResult.No)
            {
                return false;
            }

            return null;
        }

        protected override DialogResult ShowDialogCore(FolderBrowserDialog dialog, Window parent)
        {
            IntPtr parentHandle = new WindowInteropHelper(parent).Handle;
            NativeWindow nativeWindow = new();
            nativeWindow.AssignHandle(parentHandle);
            
            return dialog.ShowDialog(nativeWindow);
        }
    }
}
