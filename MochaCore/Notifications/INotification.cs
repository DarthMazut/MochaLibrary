using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.Notifications
{
    /// <summary>
    /// Represents a single instance of local notification.
    /// </summary>
    public interface INotification
    {
        /// <summary>
        /// Alows to specify custom identifier for current instance.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Schedules the notification to be displayed immediately.
        /// </summary>
        public void Schedule();

        /// <summary>
        /// Schedules the notification to be displayed at the specified time.
        /// </summary>
        /// <param name="scheduledTime">
        /// The date and time when the notification should be displayed.
        /// </param>
        public void Schedule(DateTimeOffset scheduledTime);

        /// <summary>
        /// Occurs when the user interacts with the notification associated with this instance.
        /// </summary>
        public event EventHandler? Interacted;
    }

    /// <summary>
    /// Represents a single instance of local notification
    /// with customizable properties object.
    /// </summary>
    /// <typeparam name="T">Type of custom properties object.</typeparam>
    public interface INotification<T> : INotification where T : new()
    {
        /// <summary>
        /// Allows for notification customization on technology-agnostic side.
        /// </summary>
        public T Properties { get; set; }
    }
}
