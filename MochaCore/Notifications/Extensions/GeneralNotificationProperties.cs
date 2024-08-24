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
    public class GeneralNotificationProperties
    {
        /// <summary>
        /// Provides a static identifier for <see cref="LeftButton"/> element.
        /// </summary>
        public static readonly string LeftButtonElementId = "LeftButton";

        /// <summary>
        /// Provides a static identifier for <see cref="MiddleButton"/> element.
        /// </summary>
        public static readonly string MiddleButtonElementId = "MiddleButton";

        /// <summary>
        /// Provides a static identifier for <see cref="RightButton"/> element.
        /// </summary>
        public static readonly string RightButtonElementId = "RightButton";

        public static readonly string TextInputElementId = "TextInput";

        public static readonly string SelectebleItemsElementId = "SelectableItems";

        /// <summary>
        /// Notification header text. If set to <see langword="null"/> won't be visible.
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Notification content text. If set to <see langword="null"/> won't be visible.
        /// </summary>
        public string? Content { get; set; }

        /// <summary>
        /// Specifies path to notification image. If set to <see langword="null"/> won't be visible. 
        /// </summary>
        public string? NotificationImage { get; set; }

        /// <summary>
        /// Specifies the path to the image displayed in the notification content.
        /// If set to <see langword="null"/> won't be visible.
        /// </summary>
        public string? ContentImage { get; set; }

        /// <summary>
        /// Specifies the text for the left notification button.
        /// If set to <see langword="null"/>, the button won't be visible.
        /// </summary>
        public string? LeftButton { get; set; }

        /// <summary>
        /// Specifies the text for the middle notification button.
        /// If set to <see langword="null"/>, the button won't be visible.
        /// </summary>
        public string? MiddleButton { get; set; }

        /// <summary>
        /// Specifies the text for the right notification button.
        /// If set to <see langword="null"/>, the button won't be visible.
        /// </summary>
        public string? RightButton { get; set; }

        /// <summary>
        /// Specifies whether notification displays a text input element.
        /// </summary>
        public bool HasTextInput { get; set; }

        /// <summary>
        /// Specifies the header title for text input element.
        /// If set to <see langword="null"/>, the header won't be visible.
        /// </summary>
        public string? TextInputHeader { get; set; }

        /// <summary>
        /// Specifies the placeholder text for the text input element.
        /// If set to <see langword="null"/>, no placeholder text will be visible.
        /// </summary>
        public string? TextInputPlaceholder { get; set; }

        /// <summary>
        /// Specifies a set of selectable items (e.g. ComboBox) as id-text pairs.
        /// If set to <see langword="null"/>, no selectable elements will be visible.
        /// </summary>
        public IDictionary<string, string>? SelectableItems { get; set; }

        /// <summary>
        /// Specifies the header title for selectable items collection.
        /// If set to <see langword="null"/>, the header won't be visible.
        /// </summary>
        public string? SelectableItemsHeader { get; set; }

        /// <summary>
        /// Specifies an identifier of initially selected item for <see cref="SelectableItems"/> collection.
        /// If set to <see langword="null"/>, no item will be initially selected.
        /// </summary>
        public string? InitialSelectableItemId { get; set; }
    }
}
