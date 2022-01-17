using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.DialogsEx.Extensions
{
    /// <summary>
    /// Provides a set of properties for standard dialog.
    /// </summary>
    public class MessageDialogProperties : DialogProperties
    {
        private static readonly string _titleKey = $"__MochaLibParam__{nameof(Title)}";
        private static readonly string _messageKey = $"__MochaLibParam__{nameof(Message)}";

        /// <summary>
        /// Dialog title.
        /// </summary>
        public string? Title 
        {
            get => GetParameter<string>(_titleKey);
            set => SetParameter(_titleKey, value);
        }

        /// <summary>
        /// Dialog message.
        /// </summary>
        public string? Message
        {
            get => GetParameter<string>(_messageKey);
            set => SetParameter(_messageKey, value);
        }
    }
}
