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
    /// <summary>
    /// Provides a standard implementation of <see cref="IDialogModule{T}"/> for WPF <see cref="FolderBrowserDialog"/> class.
    /// </summary>
    public class BrowseFolderDialogModule : BrowseBaseDialogModule<FolderBrowserDialog, DialogResult, BrowseFolderDialogProperties>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BrowseFolderDialogModule"/> class.
        /// </summary>
        /// <param name="application">WPF application object.</param>
        public BrowseFolderDialogModule(Application application) : base(application, new BrowseFolderDialogProperties(), new FolderBrowserDialog()) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BrowseFolderDialogModule"/> class.
        /// </summary>
        /// <param name="application">WPF application object.</param>
        /// <param name="properties">Statically typed properties object which serves for configuration of this module.</param>
        public BrowseFolderDialogModule(Application application, BrowseFolderDialogProperties properties) : base(application, properties, new FolderBrowserDialog()) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BrowseFolderDialogModule"/> class.
        /// </summary>
        /// <param name="application">WPF application object.</param>
        /// <param name="properties">Statically typed properties object which serves for configuration of this module.</param>
        /// <param name="dialog">Technology-specific dialog object which is represented by this module.</param>
        public BrowseFolderDialogModule(Application application, BrowseFolderDialogProperties properties, FolderBrowserDialog dialog) : base(application, properties, dialog) { }

        /// <inheritdoc/>
        protected override void ApplyPropertiesCore(FolderBrowserDialog dialog, BrowseFolderDialogProperties properties)
        {
            dialog.SelectedPath = properties.InitialDirectory;
            dialog.UseDescriptionForTitle = true;
            dialog.Description = properties.Title;
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        protected override DialogResult ShowDialogCore(FolderBrowserDialog dialog, Window parent)
        {
            IntPtr parentHandle = new WindowInteropHelper(parent).Handle;
            NativeWindow nativeWindow = new();
            nativeWindow.AssignHandle(parentHandle);
            
            return dialog.ShowDialog(nativeWindow);
        }
    }
}
