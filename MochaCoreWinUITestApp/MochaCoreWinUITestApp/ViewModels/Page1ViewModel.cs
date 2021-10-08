using MochaCore.Navigation;
using MochaCore.Utils;
using System.ComponentModel;

namespace MochaCoreWinUITestApp.ViewModels
{
    public class Page1ViewModel : INavigatable, INotifyPropertyChanged, IOnNavigatedTo
    {
        private PropertyNotifier _notifier;

        public Page1ViewModel()
        {
            Navigator = new(this, NavigationServices.MainNavigationService);
            _notifier = new(this);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public Navigator Navigator { get; }

        private string? _text;
        public string? Text
        {
            get => _text;
            set => _notifier.ChangeAndNotify(value, ref _text);
        }

        public void OnNavigatedTo(NavigationData navigationData)
        {
            Text = "Hello There Page 1 ;)";
        }
    }
}
