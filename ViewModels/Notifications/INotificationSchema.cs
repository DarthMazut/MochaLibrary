using CommunityToolkit.Mvvm.ComponentModel;
using MochaCore.Notifications;
using System.Runtime.CompilerServices;

namespace ViewModels.Notifications
{
    public interface INotificationSchema
    {
        public INotification CreateNotification();
    }
}
