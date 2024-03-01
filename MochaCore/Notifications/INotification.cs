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
    public interface INotification : IDisposable
    {
        /// <summary>
        /// Unique identifier for current instance.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// 
        /// </summary>
        public string? Tag { get; }

        /// <summary>
        /// Notification schedule time.
        /// </summary>
        public DateTimeOffset? ScheduledTime { get; }

        /// <summary>
        /// Indicates whether the notification has been displayed.
        /// Once the notification is displayed, it can no longer be scheduled.
        /// </summary>
        public bool Displayed { get; }

        /// <summary>
        /// Indicates whether this instance has been disposed.
        /// </summary>
        public bool IsDisposed { get; }

        /// <summary>
        /// Occurs when the user interacts with the notification associated with this instance.
        /// </summary>
        public event EventHandler<NotificationInteractedEventArgs> Interacted;

        /// <summary>
        /// Occurs when this instance is disposed.
        /// </summary>
        public event EventHandler? Disposed;

        /// <summary>
        /// Schedules the notification to be displayed immediately.
        /// </summary>
        public void Schedule();

        public void Schedule(string tag);

        /// <summary>
        /// Schedules the notification to be displayed at the specified time.
        /// </summary>
        /// <param name="scheduledTime">
        /// The date and time when the notification should be displayed.
        /// </param>
        public void Schedule(DateTimeOffset scheduledTime);

        public void Schedule(DateTimeOffset scheduledTime, string tag);
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

    /// <summary>
    /// Represents a single instance of local notification
    /// with customizable properties object and interaction event args.
    /// </summary>
    /// <typeparam name="TProps">Type of custom properties object.</typeparam>
    /// <typeparam name="TArgs">Type of interaction event args.</typeparam>
    public interface INotification<TProps, TArgs> : INotification<TProps> where TProps : new()
    {
        /// <summary>
        /// Occurs when the user interacts with the notification associated with this instance.
        /// </summary>
        public new event EventHandler<TArgs?>? Interacted;
    }
}
