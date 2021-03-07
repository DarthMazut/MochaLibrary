using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MochaWPF
{
    /// <summary>
    /// Maps results from <see cref="MessageBoxResult"/> to nullable <see langword="bool"/> values.
    /// </summary>
    public class MessageBoxResultMapper
    {
        /// <summary>
        /// Holds a maped value for <see cref="MessageBoxResult.None"/>.
        /// </summary>
        public bool? None { get; set; } = null;

        /// <summary>
        /// Holds a maped value for <see cref="MessageBoxResult.OK"/>.
        /// </summary>
        public bool? OK { get; set; } = true;

        /// <summary>
        /// Holds a maped value for <see cref="MessageBoxResult.Cancel"/>.
        /// </summary>
        public bool? Cancel { get; set; } = false;

        /// <summary>
        /// Holds a maped value for <see cref="MessageBoxResult.Yes"/>.
        /// </summary>
        public bool? Yes { get; set; } = true;

        /// <summary>
        /// Holds a maped value for <see cref="MessageBoxResult.No"/>.
        /// </summary>
        public bool? No { get; set; } = false;

        /// <summary>
        /// Holds a value used when none of specified mapping is selected.
        /// </summary>
        public bool? Default { get; set; } = null;

        /// <summary>
        /// Maps given <see cref="MessageBoxResult"/> value to nullable <see langword="bool"/> object,
        /// based on set properties.
        /// </summary>
        /// <param name="messageBoxResult">Value to be mapped.</param>
        public bool? MessageBoxResultToBoolean(MessageBoxResult messageBoxResult)
        {
            switch (messageBoxResult)
            {
                case MessageBoxResult.None:
                    return None;
                case MessageBoxResult.OK:
                    return OK;
                case MessageBoxResult.Cancel:
                    return Cancel;
                case MessageBoxResult.Yes:
                    return Yes;
                case MessageBoxResult.No:
                    return No;
                default:
                    return Default;
            }
        }
    }
}
