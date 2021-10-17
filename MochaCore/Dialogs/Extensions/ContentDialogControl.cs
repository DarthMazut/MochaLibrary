using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.Dialogs.Extensions
{
    /// <summary>
    /// Exposes API for file dialog interaction.
    /// </summary>
    public class ContentDialogControl : DialogControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentDialogControl"/> class.
        /// </summary>
        /// <param name="dialog">
        /// An <see cref="IDialog{T}"/> implementation which hosts this instance.
        /// Pass <see langword="this"/> here.
        /// </param>
        public ContentDialogControl(IDialog dialog) : base(dialog) { }

        public object? Content 
        {
            get => GetParameter<object>(ResolveCurrentKey());
            set => SetParameter(ResolveCurrentKey(), value);
        }

        public string? PrimaryButtonText 
        {
            get => GetParameter<string>(ResolveCurrentKey());
            set => SetParameter(ResolveCurrentKey(), value);
        }

        public string? SecondaryButtonText 
        {
            get => GetParameter<string>(ResolveCurrentKey());
            set => SetParameter(ResolveCurrentKey(), value);
        }

        public string? CloseButtonText 
        {
            get => GetParameter<string>(ResolveCurrentKey());
            set => SetParameter(ResolveCurrentKey(), value);
        }

        private string ResolveCurrentKey([CallerMemberName] string? propertyName = null)
        {
            return $"__MochaLibParam__{propertyName}";
        }

    }
}
