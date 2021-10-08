using Microsoft.UI.Xaml;
using MochaCore.Navigation;
using MochaCore.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCoreWinUITestApp.ViewModels
{
    public class MainWindowViewModel : INavigatable, INotifyPropertyChanged
    {
        private PropertyNotifier _notifier;

        public MainWindowViewModel()
        {
            _notifier = new PropertyNotifier(this);
            Navigator = new(this, NavigationServices.MainNavigationService);

            NavigationServices.MainNavigationService.NavigationRequested += NavigationRequested;
            NavigationItemSelectionChangedCommand = new SimpleCommand(NavigationItemSelectionChanged);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public Navigator Navigator { get; }

        public ObservableCollection<PageInfo> PageItems { get; } = new()
        {
            Pages.Page1,
            Pages.Page2
        };

        private object? _content;
        public object? Content
        {
            get => _content;
            set => _notifier.ChangeAndNotify(value, ref _content);
        }

        public SimpleCommand NavigationItemSelectionChangedCommand { get; set; }

        private void NavigationRequested(object? sender, NavigationData e)
        {
            Content = e.RequestedModule.View;
        }

        private async void NavigationItemSelectionChanged(object? obj)
        {
            _ = await Navigator.NavigateAsync((obj as PageInfo).NavigationModule);
        }
    }
}
