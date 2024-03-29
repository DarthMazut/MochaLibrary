﻿using Microsoft.Win32;
using MochaCore.Dialogs;
using MochaCore.Dialogs.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MochaWPF.Dialogs
{
    /// <summary>
    /// Provides a standard implementation of <see cref="IDialogModule{T}"/> for WPF <see cref="OpenFileDialog"/> class.
    /// </summary>
    public class OpenFileDialogModule : BrowseBaseDialogModule<OpenFileDialog, bool?, OpenFileDialogProperties>
    {
        public OpenFileDialogModule(Application application) : base(application, new OpenFileDialogProperties(), new OpenFileDialog()) { }

        public OpenFileDialogModule(Application application, OpenFileDialogProperties properties) : base(application, properties, new OpenFileDialog()) { }

        public OpenFileDialogModule(Application application, OpenFileDialogProperties properties, OpenFileDialog dialog) : base(application, properties, dialog) { }

        /// <inheritdoc/>
        protected override void ApplyPropertiesCore(OpenFileDialog dialog, OpenFileDialogProperties properties)
        {
            dialog.Title = properties.Title;
            dialog.Filter = properties.Filters.ToWpfFilterFormat();
            dialog.InitialDirectory = properties.InitialDirectory;
            dialog.Multiselect = properties.MultipleSelection;
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        protected override bool? ShowDialogCore(OpenFileDialog dialog, Window parent)
        {
            return dialog.ShowDialog(parent);
        }
    }
}
