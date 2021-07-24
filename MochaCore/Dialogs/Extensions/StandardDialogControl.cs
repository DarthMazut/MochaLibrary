using System;
using System.Collections.Generic;
using System.Text;

namespace MochaCore.Dialogs.Extensions
{
    /// <summary>
    /// Exposes API for standard dialog interaction.
    /// </summary>
    public class StandardDialogControl : DialogControl
    {
        /// <summary>
        /// Message for displaying dialog.
        /// </summary>
        public string? Message
        {
            get => GetParameter<string>(StandardDialogParameters.Message);
            set => SetParameter(StandardDialogParameters.Message, value);
        }

        /// <summary>
        /// Icon for displaying dialog.
        /// </summary>
        public string? Icon
        {
            get => GetParameter<string>(StandardDialogParameters.Icon);
            set => SetParameter(StandardDialogParameters.Icon, value);
        }

        /// <summary>
        /// Defines a predefined buttons set for displaying dialog.
        /// </summary>
        public string? PredefinedButtons
        {
            get => GetParameter<string>(StandardDialogParameters.PredefinedButtons);
            set => SetParameter(StandardDialogParameters.PredefinedButtons, value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StandardDialogControl"/> class.
        /// </summary>
        /// <param name="dialog">
        /// An <see cref="IDialog{T}"/> implementation which hosts this instance.
        /// Pass <see langword="this"/> here.
        /// </param>
        public StandardDialogControl(IDialog dialog) : base(dialog) { }
    }
}
