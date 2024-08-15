using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MochaCore.Dialogs;
using MochaCore.Dialogs.Extensions;
using MochaCore.Dispatching;
using ModelX.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModelsX.Dialogs;

namespace ViewModelsX.Pages.Dialogs
{
    public partial class SystemDialogsTabViewModel : ObservableObject
    {
        private readonly DialogsPageViewModel _parentViewModel;

        public SystemDialogsTabViewModel(DialogsPageViewModel parentViewModel)
        {
            _parentViewModel = parentViewModel;
        }

        public SystemDialogPaneViewModel PaneViewModel { get; } = new();

        [ObservableProperty]
        private ObservableCollection<SystemDialog> _dialogs = [];

        [ObservableProperty]
        private SystemDialog? _selectedDialog;

        [ObservableProperty]
        private bool _isPaneOpen;

        [ObservableProperty]
        private string? _title;

        [ObservableProperty]
        private string? _initialDirectory;

        partial void OnSelectedDialogChanged(SystemDialog? value)
        {
            Title = value?.Title;
            InitialDirectory = value?.InitialDirectory;
        }

        partial void OnTitleChanged(string? value)
        {
            if (SelectedDialog is null)
            {
                return;
            }

            SelectedDialog.Title = value;
        }

        partial void OnInitialDirectoryChanged(string? value)
        {
            if (SelectedDialog is null)
            {
                return;
            }

            SelectedDialog.InitialDirectory = value;
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
        private async Task CreateDialog()
        {
            using ICustomDialogModule<CreateDialogDialogProperties> createDialogDialogModule = AppDialogs.CreateDialogDialog.Module;
            bool? result = await createDialogDialogModule.ShowModalAsync(_parentViewModel.Navigator.Module.View);
            if (result is true)
            {
                SystemDialog newDialog = createDialogDialogModule.Properties.CreatedDialog;
                Dialogs.Add(newDialog);
                SelectedDialog = newDialog;
                IsPaneOpen = true;
            }
        }

        [RelayCommand]
        private void ClosePane() => IsPaneOpen = false;

        [RelayCommand]
        private Task ShowDialog()
        {
            return SelectedDialog?.SafeShow(_parentViewModel.Navigator.Module.View) ?? Task.CompletedTask;
        }

        [RelayCommand]
        private void ShowDialogDetails()
        {
            IsPaneOpen = true;
        }

        [RelayCommand]
        private async Task DisposeDialog()
        {
            if (SelectedDialog is null)
            {
                return;
            }

            SelectedDialog.CoreModule.Dispose();
            await Task.Delay(3000);
            Dialogs.Remove(SelectedDialog);
        }
    }
}
