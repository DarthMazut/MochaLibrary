using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MochaCore.Dialogs.Extensions;
using ModelX.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModelsX.Pages.Dialogs
{
    public partial class SystemDialogPaneViewModel : ObservableObject
    {
        [ObservableProperty]
        private SystemDialog? _selectedDialog;

        [ObservableProperty]
        private string? _title;

        [ObservableProperty]
        private string? _initialDirectory;

        [ObservableProperty]
        private bool _multiselection;

        [ObservableProperty]
        private List<ExtensionFilter> _filters = [];

        [ObservableProperty]
        private ExtensionFilter? _selectedFilter;

        partial void OnSelectedDialogChanged(SystemDialog? value)
        {
            Title = value?.Title;
            InitialDirectory = value?.InitialDirectory;
            Multiselection = value?.Multiselection ?? false;
            Filters = [.. value?.Filters];
        }

        partial void OnTitleChanged(string? value) => SelectedDialog!.Title = value;

        partial void OnInitialDirectoryChanged(string? value) => SelectedDialog!.InitialDirectory = value;

        partial void OnMultiselectionChanged(bool value) => SelectedDialog!.Multiselection = value;

        [RelayCommand]
        private void SelectSpecialFolder(string name)
        {
            if (Enum.TryParse(name, out Environment.SpecialFolder sf))
            {
                InitialDirectory = Environment.GetFolderPath(sf);
            }
        }

        [RelayCommand]
        private void AddFilter(string filterName)
        {
            Filters = [.. Filters, new ExtensionFilter(filterName, [])];
            SelectedDialog!.Filters = Filters;
        }

        [RelayCommand]
        private void AddExtension(string extension)
        {
            SelectedFilter?.Extensions.Add(extension);
            Filters = [.. Filters];
        }
    }
}
