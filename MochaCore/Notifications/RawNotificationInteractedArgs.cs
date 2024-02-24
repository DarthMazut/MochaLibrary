using System;
using System.Collections;
using System.Collections.Generic;

namespace MochaCore.Notifications
{
    /// <summary>
    /// Provides technology-agnostic generic arguments for application notification interaction.
    /// </summary>
    public class RawNotificationInteractedArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RawNotificationInteractedArgs"/> class.
        /// </summary>
        /// <param name="rawArgs">
        /// Contains projection of original arguments into <see cref="IReadOnlyDictionary{TKey, TValue}"/>.
        /// </param>
        /// <param name="notificationId">Identifier of interacted notification.</param>
        public RawNotificationInteractedArgs(IReadOnlyDictionary<string, object> rawArgs, string? notificationId)
        {
            RawArgs = rawArgs;
            NotificationId = notificationId;
        }

        /// <summary>
        /// Identifier of interacted notification.
        /// </summary>
        public string? NotificationId { get; }

        /// <summary>
        /// Contains projection of original arguments into <see cref="IReadOnlyDictionary{TKey, TValue}"/>.
        /// </summary>
        public IReadOnlyDictionary<string, object> RawArgs { get; }
    }
}