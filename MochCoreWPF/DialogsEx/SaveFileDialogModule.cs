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
    /// <summary>
    /// Provides a standard implementation of <see cref="IDialogModule{T}"/> for WPF <see cref="SaveFileDialog"/> class.
    /// </summary>
    public class SaveFileDialogModule : BrowseBaseDialogModule<SaveFileDialog, bool?, SaveFileDialogProperties>
    {
        public SaveFileDialogModule(Application application) : base(application, new SaveFileDialogProperties(), new SaveFileDialog()) { }

        public SaveFileDialogModule(Application application, SaveFileDialogProperties properties) : base(application, properties, new SaveFileDialog()) { }

        public SaveFileDialogModule(Application application, SaveFileDialogProperties properties, SaveFileDialog dialog) : base(application, properties, dialog) { }

        /// <inheritdoc/>
        protected override void ApplyPropertiesCore(SaveFileDialog dialog, SaveFileDialogProperties properties)
        {
            dialog.Title = properties.Title;
            dialog.Filter = properties.Filters.ToWpfFilterFormat();
        }

        /// <inheritdoc/>
        protected override bool? HandleResultCore(SaveFileDialog dialog, bool? result, SaveFileDialogProperties properties)
        {
            properties.SelectedPath = dialog.FileName;
            return result;
        }

        /// <inheritdoc/>
        protected override bool? ShowDialogCore(SaveFileDialog dialog, Window parent)
        {
            return dialog.ShowDialog(parent);
        }
    }
}
