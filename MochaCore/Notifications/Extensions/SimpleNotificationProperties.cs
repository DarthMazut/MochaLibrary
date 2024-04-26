using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.Notifications.Extensions
{
    /// <summary>
    /// Provides properties for basic notification.
    /// </summary>
    public class SimpleNotificationProperties
    {
        /// <summary>
        /// Notification header text. If set to <see langword="null"/> won't be visible.
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Notification content text. If set to <see langword="null"/> won't be visible.
        /// </summary>
        public string? Content { get; set; }

        /// <summary>
        /// Specifies notification image. If set to <see langword="null"/> won't be visible. 
        /// </summary>
        public string? Image { get; set; }
    }
}
