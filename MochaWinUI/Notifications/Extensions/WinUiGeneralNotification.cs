using Microsoft.Windows.AppNotifications;
using Microsoft.Windows.AppNotifications.Builder;
using MochaCore.Notifications;
using MochaCore.Notifications.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Notifications;

namespace MochaWinUI.Notifications.Extensions
{
    /// <summary>
    /// Provides implementation of general notification for WinUI.
    /// </summary>
    public class WinUiGeneralNotification : WinUiNotification<GeneralNotificationProperties>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WinUiGeneralNotification"/> class.
        /// </summary>
        /// <param name="registrationId">
        /// The identifier assigned during registration with the <see cref="NotificationManager"/>
        /// </param>
        public WinUiGeneralNotification(string registrationId) : base(registrationId) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="WinUiGeneralNotification"/> class.
        /// </summary>
        /// <param name="creationData"></param>
        protected WinUiGeneralNotification(NotificationCreationData creationData) : base(creationData) { }

        /// <inheritdoc/>
        protected override string CreateNotificationDefinition()
        {
            AppNotificationBuilder builder = new();
            builder.AddNotificationArguments(this);
            if (Properties.Title is string title)
            {
                builder.AddText(title);
            }
            
            if (Properties.Content is string content)
            {
                builder.AddText(content);
            }

            if (Properties.Image is string imageUri)
            {
                builder.SetAppLogoOverride(new Uri(imageUri));
            }

            if (Properties.LeftButton is string leftButtonText)
            {
                builder.AddButton(new AppNotificationButton(leftButtonText)
                    .AddElementArguments(this, nameof(Properties.LeftButton)));
            }

            if (Properties.RightButton is string rightButtonText)
            {
                builder.AddButton(new AppNotificationButton(rightButtonText)
                    .AddElementArguments(this, nameof(Properties.RightButton)));
            }

            return builder.BuildNotification().Payload;
        }

        /// <inheritdoc/>
        protected override INotification CreateForExistingNotification(NotificationCreationData creationData)
            => new WinUiGeneralNotification(creationData);
    }
}
