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
        [NotifyPropertyChangedFor(nameof(IsMultiselectionEnabled))]
        [NotifyPropertyChangedFor(nameof(IsFilterTableEnabled))]
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
        [NotifyPropertyChangedFor(nameof(CanAddExtension))]
        [NotifyPropertyChangedFor(nameof(CanRemoveFilter))]
        private ExtensionFilter? _selectedFilter;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CanAddFilter))]
        [NotifyPropertyChangedFor(nameof(CanAddExtension))]
        [NotifyPropertyChangedFor(nameof(CanRemoveExtension))]
        private string _filterInput = string.Empty;

        public bool CanAddFilter => !string.IsNullOrWhiteSpace(FilterInput);

        public bool CanAddExtension =>
            SelectedFilter is not null &&
            !SelectedFilter.Extensions.Contains(FilterInput) &&
            !string.IsNullOrWhiteSpace(FilterInput);

        public bool CanRemoveFilter => SelectedFilter is not null;

        public bool CanRemoveExtension => SelectedFilter is not null && SelectedFilter.Extensions.Contains(FilterInput);

        public bool IsMultiselectionEnabled => SelectedDialog?.Type == SystemDialogType.OpenDialog;

        public bool IsFilterTableEnabled => SelectedDialog?.Type != SystemDialogType.BrowseDialog;

        partial void OnSelectedDialogChanged(SystemDialog? value)
        {
            Title = value?.Title;
            InitialDirectory = value?.InitialDirectory;
            Multiselection = value?.Multiselection ?? false;
            Filters = [.. value?.Filters ?? []];
        }

        partial void OnTitleChanged(string? value)
        {
            if (SelectedDialog is not null)
            {
                SelectedDialog.Title = value;
            } 
        }

        partial void OnInitialDirectoryChanged(string? value)
        {
            if (SelectedDialog is not null)
            {
                SelectedDialog.InitialDirectory = value;
            }
        }

        partial void OnMultiselectionChanged(bool value)
        {
            if (SelectedDialog is not null)
            {
                SelectedDialog!.Multiselection = value;
            }
        }

        [RelayCommand]
        private void SelectSpecialFolder(string name)
        {
            if (Enum.TryParse(name, out Environment.SpecialFolder sf))
            {
                InitialDirectory = Environment.GetFolderPath(sf);
            }
        }

        [RelayCommand]
        private void AddFilter()
        {
            Filters = [.. Filters, new ExtensionFilter(FilterInput, [])];
            SelectedDialog!.Filters = Filters;
            FilterInput = string.Empty;
        }

        [RelayCommand]
        private void AddExtension()
        {
            SelectedFilter?.Extensions.Add(FilterInput);
            Filters = [.. Filters];
            FilterInput = string.Empty;
        }

        [RelayCommand]
        private void RemoveFilter()
        {
            Filters = [..Filters.Except([SelectedFilter])];
        }

        [RelayCommand]
        private void RemoveExtension()
        {
            SelectedFilter?.Extensions.Remove(FilterInput);
            Filters = [.. Filters];
            FilterInput = string.Empty;
        }
    }
}
