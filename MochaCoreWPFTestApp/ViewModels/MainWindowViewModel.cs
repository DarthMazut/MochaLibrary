using MochaCore.Navigation;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCoreWPFTestApp.ViewModels
{
    class MainWindowViewModel : BindableBase, INavigatable
    {
        private readonly List<string> _menuItems = new List<string>
        {
            Pages.Page1Id,
            Pages.Page2Id
        };

        public MainWindowViewModel()
        {
            _filteredMenuItems = _menuItems;

            Navigator = new Navigator(this, NavigationServices.MainNavigationService);
            NavigationServices.MainNavigationService.NavigationRequested += (s, e) => Content = e.RequestedModule.View;
        }

        public Navigator Navigator { get; }

        private string _searchText = string.Empty;
        public string SearchText
        {
            get => _searchText;
            set
            {
                SetProperty(ref _searchText, value);
                FilteredMenuItems = value.Length > 0 ? _menuItems.Where(i => i.Contains(value)).ToList() : _menuItems;
                IsError = FilteredMenuItems.Count == 0;
            }
        }

        private bool _isError;
        public bool IsError
        {
            get => _isError;
            set => SetProperty(ref _isError, value);
        }

        private List<string> _filteredMenuItems;
        public List<string> FilteredMenuItems
        {
            get => _filteredMenuItems;
            set => SetProperty(ref _filteredMenuItems, value);
        }

        private object? _selectedMenuItem;
        public object? SelectedMenuItem
        {
            get => _selectedMenuItem;
            set => SetProperty(ref _selectedMenuItem, value);
        }

        private object? _content;
        public object? Content
        {
            get => _content;
            set => SetProperty(ref _content, value);
        }

        private DelegateCommand _selectedMenuItemChangedCommand;
        public DelegateCommand SelectedMenuItemChangedCommand => _selectedMenuItemChangedCommand ?? (_selectedMenuItemChangedCommand = new DelegateCommand(SelectedMenuItemChanged));

        private async void SelectedMenuItemChanged()
        {
            await Navigator.NavigateAsync(NavigationManager.FetchModule(SelectedMenuItem?.ToString() ?? string.Empty));
        }
    }
}
