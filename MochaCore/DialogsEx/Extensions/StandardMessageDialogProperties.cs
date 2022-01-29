using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.DialogsEx.Extensions
{
    /// <summary>
    /// Provides a set of properties for standard dialog.
    /// </summary>
    public class StandardMessageDialogProperties : DialogProperties // Add Standard* prefix...
    {
        // I don't think this must be deriving from DialogProperties

        public StandardMessageDialogProperties()
        {
            Buttons = new();
        }

        /// <summary>
        /// Dialog title.
        /// </summary>
        public string? Title 
        {
            get => GetParameter<string>($"__MochaLibParam__{MethodBase.GetCurrentMethod()?.Name}");
            set => SetParameter($"__MochaLibParam__{MethodBase.GetCurrentMethod()?.Name}", value);
        }

        /// <summary>
        /// Dialog message.
        /// </summary>
        public string? Message
        {
            get => GetParameter<string>($"__MochaLibParam__{MethodBase.GetCurrentMethod()?.Name}");
            set => SetParameter($"__MochaLibParam__{MethodBase.GetCurrentMethod()?.Name}", value);
        }

        /// <summary>
        /// Allows for configuration of dialog icon.
        /// </summary>
        public string? Icon 
        {
            get => GetParameter<string>($"__MochaLibParam__{MethodBase.GetCurrentMethod()?.Name}");
            set => SetParameter($"__MochaLibParam__{MethodBase.GetCurrentMethod()?.Name}", value);
        }

        /// <summary>
        /// Buttons definition.
        /// </summary>
        public MessageDialogButtons? Buttons
        {
            get => GetParameter<MessageDialogButtons>($"__MochaLibParam__{nameof(Buttons)}");
            set => SetParameter($"__MochaLibParam__{nameof(Buttons)}", value);
        } 
    }


}
