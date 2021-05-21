using System;
using System.Collections.Generic;
using System.Text;

namespace Mocha.Dialogs.Extensions
{
    /// <summary>
    /// Exposes API for file dialog interaction.
    /// </summary>
    public class FileDialogControl : DialogControl
    {
        /// <summary>
        /// Returns new instance of <see cref="FileDialogControl"/> class.
        /// </summary>
        /// <param name="dialog">
        /// An <see cref="IDialog{T}"/> implementation which hosts this instance.
        /// Pass <see langword="this"/> here.
        /// </param>
        public FileDialogControl(IDialog dialog) : base(dialog) { }

        /// <summary>
        /// Path selected as a result of interaction with file dialog.
        /// </summary>
        public string SelectedPath
        {
            get => GetParameter<string>(StandardDialogParameters.SelectedPath);
            set => SetParameter(StandardDialogParameters.SelectedPath, value);
        }

        /// <summary>
        /// Defines a filter for Open/Save dialog.
        /// </summary>
        public string Filter
        {
            get => GetParameter<string>(StandardDialogParameters.Filter);
            set => SetParameter(StandardDialogParameters.Filter, value);
        }

        /// <summary>
        /// Sets initial directory for Open/Save dialog.
        /// </summary>
        public string InitialDirectory
        {
            get => GetParameter<string>(StandardDialogParameters.InitialDirectory);
            set => SetParameter(StandardDialogParameters.InitialDirectory, value);
        }

        /// <summary>
        /// Extension added automatically in case none was specified.
        /// </summary>
        public string DefaultExtension
        {
            get => GetParameter<string>(StandardDialogParameters.DefaultExtension);
            set => SetParameter(StandardDialogParameters.DefaultExtension, value);
        }


    }
}
