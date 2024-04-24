using CommunityToolkit.Mvvm.ComponentModel;

namespace ViewModels.Notifications
{
    public partial class NotificationHeroImageSchema : ObservableObject, INotificationSchema
    {
        [ObservableProperty]
        private string? _title;

        [ObservableProperty]
        private string? _content;

        [ObservableProperty]
        private string? _imageUri;

        public override string ToString() => "Hero image schema";
    }
}
