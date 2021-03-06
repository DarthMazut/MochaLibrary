using System;
using System.Collections.Generic;
using System.Text;

namespace Mocha.Dialogs
{
    /// <summary>
    /// Contains available behaviors for associated dialog.
    /// </summary>
    public class DialogBehaviors
    {
        /// <summary>
        /// Allows to close a dialog by <see cref="IDialog"/> implementation.
        /// </summary>
        public Action Close { get; set; } = () => { };
    }
}
