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

            if (Properties.NotificationImage is string imageUri)
            {
                builder.SetAppLogoOverride(new Uri(imageUri));
            }

            if (Properties.LeftButton is string leftButtonText)
            {
                builder.AddButton(new AppNotificationButton(leftButtonText)
                    .AddElementArguments(this, GeneralNotificationProperties.LeftButtonElementId));
            }

            if (Properties.MiddleButton is string middleButtonText)
            {
                builder.AddButton(new AppNotificationButton(middleButtonText)
                    .AddElementArguments(this, GeneralNotificationProperties.MiddleButtonElementId));
            }

            if (Properties.RightButton is string rightButtonText)
            {
                builder.AddButton(new AppNotificationButton(rightButtonText)
                    .AddElementArguments(this, GeneralNotificationProperties.RightButtonElementId));
            }

            if (Properties.ContentImage is string contentImagePath)
            {
                builder.SetHeroImage(new Uri(contentImagePath));
            }

            if (Properties.SelectableItems is IDictionary<string, string> selectableItems)
            {
                builder.AddComboBox(new AppNotificationComboBox(GeneralNotificationProperties.SelectebleItemsElementId)
                {
                    Title = Properties.SelectableItemsHeader,
                    Items = Properties.SelectableItems,
                    SelectedItem = Properties.InitialSelectableItemId
                });
            }

            if (Properties.HasTextInput)
            {
                builder.AddTextBox(
                    GeneralNotificationProperties.TextInputElementId,
                    Properties.TextInputPlaceholder,
                    Properties.TextInputHeader);
            }

            return builder.BuildNotification().Payload;
        }

        /// <inheritdoc/>
        protected override NotificationInteractedEventArgs CreateArgsFromInteractedEvent(AppNotificationActivatedEventArgs args)
        {
            args.UserInput.TryGetValue(GeneralNotificationProperties.TextInputElementId, out string? textInput);
            args.UserInput.TryGetValue(GeneralNotificationProperties.SelectebleItemsElementId, out string? selectedItem);

            return new(
                CreateInteractedNotification(args),
                args.Arguments[InvokedItemIdKey],
                args.AsDictionary(),
                args)
            {
                TextInput = textInput,
                SelectedItemId = selectedItem,
                IsActivationEvent = args.CheckLaunchingArg()
            };
        }

        /// <inheritdoc/>
        protected override INotification CreateForExistingNotification(NotificationCreationData creationData)
            => new WinUiGeneralNotification(creationData);
    }
}
