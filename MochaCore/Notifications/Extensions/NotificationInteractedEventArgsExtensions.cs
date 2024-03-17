using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.Notifications.Extensions
{
    /// <summary>
    /// Contains extension methods for <see cref="NotificationInteractedEventArgs"/>.
    /// </summary>
    public static class NotificationInteractedEventArgsExtensions
    {
        /// <summary>
        /// Returns a shallow copy of <see cref="NotificationInteractedEventArgs"/> object with
        /// <see cref="NotificationInteractedEventArgs.Notification"/> value replaced by provided reference.
        /// </summary>
        /// <param name="args">Arguments to be copied.</param>
        /// <param name="notification">Notification that replaces current value.</param>
        public static NotificationInteractedEventArgs WithNotification(this NotificationInteractedEventArgs args, INotification notification)
            => new (
                notification,
                args.InvokedItemId,
                args.AsDictionary,
                args.RawArgs)
                {
                    SelectedDate = args.SelectedDate,
                    SelectedItemId = args.SelectedItemId,
                    TextInput = args.TextInput
                };

        /// <summary>
        /// Returns a shallow copy of <see cref="NotificationInteractedEventArgs{T}"/> object with
        /// <see cref="NotificationInteractedEventArgs.Notification"/> value replaced by provided reference.
        /// </summary>
        /// <typeparam name="T">Type of detailed interaction data.</typeparam>
        /// <param name="args">Arguments to be copied.</param>
        /// <param name="notification">Notification that replaces current value.</param>
        public static NotificationInteractedEventArgs<T> WithNotification<T>(
            this NotificationInteractedEventArgs<T> args,
            INotification notification) => new(
                notification,
                args.InvokedItemId,
                args.AsDictionary,
                args.RawArgs,
                args.InteractionData)
            {
                SelectedDate = args.SelectedDate,
                SelectedItemId = args.SelectedItemId,
                TextInput = args.TextInput
            };
    }
}
