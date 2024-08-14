using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MochaCore.Dialogs;
using MochaCore.Dialogs.Extensions;
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

        partial void OnSelectedDialogChanged(SystemDialog? value)
        {
            switch (SelectedDialog?.Module)
            {
                case IDialogModule<SaveFileDialogProperties> saveModule:
                    Title = saveModule.Properties.Title;
                    InitialDirectory = saveModule.Properties.InitialDirectory;
                    break;
                case IDialogModule<OpenFileDialogProperties> openModule:
                    Title = openModule.Properties.Title;
                    InitialDirectory = openModule.Properties.InitialDirectory;
                    break;
                case IDialogModule<BrowseFolderDialogProperties> browseModule:
                    Title = browseModule.Properties.Title;
                    InitialDirectory = browseModule.Properties.InitialDirectory;
                    break;
                default:
                    break;
            }
        }

        partial void OnTitleChanged(string? value)
        {
            switch (SelectedDialog?.Module)
            {
                case IDialogModule<SaveFileDialogProperties> saveModule:
                    saveModule.Properties.Title = value;
                    break;
                case IDialogModule<OpenFileDialogProperties> openModule:
                    openModule.Properties.Title = value;
                    break;
                case IDialogModule<BrowseFolderDialogProperties> browseModule:
                    browseModule.Properties.Title = value;
                    break;
                default:
                    break;
            }
        }

        [ObservableProperty]
        private string? _initialDirectory;

        partial void OnInitialDirectoryChanged(string? value)
        {
            switch (SelectedDialog?.Module)
            {
                case IDialogModule<SaveFileDialogProperties> saveModule:
                    saveModule.Properties.InitialDirectory = value;
                    break;
                case IDialogModule<OpenFileDialogProperties> openModule:
                    openModule.Properties.InitialDirectory = value;
                    break;
                case IDialogModule<BrowseFolderDialogProperties> browseModule:
                    browseModule.Properties.InitialDirectory = value;
                    break;
                default:
                    break;
            }
        }

        [RelayCommand]
        private void SelectSpecialFolder(string name)
        {
            if (Enum.TryParse(name, out Environment.SpecialFolder sf))
            {
                InitialDirectory = Environment.GetFolderPath(sf);
                switch (SelectedDialog?.Module)
                {
                    case IDialogModule<SaveFileDialogProperties> saveModule:
                        saveModule.Properties.TrySetInitialDirectory(sf);
                        break;
                    case IDialogModule<OpenFileDialogProperties> openModule:
                        openModule.Properties.TrySetInitialDirectory(sf);
                        break;
                    case IDialogModule<BrowseFolderDialogProperties> browseModule:
                        browseModule.Properties.TrySetInitialDirectory(sf);
                        break;
                    default:
                        break;
                }
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
        private async Task ShowDialog()
        {
            await (SelectedDialog?.Module.ShowModalAsync(_parentViewModel.Navigator.Module.View) ?? Task.CompletedTask);
        }

        [RelayCommand]
        private void ShowDialogDetails()
        {
            IsPaneOpen = true;
        }

        [RelayCommand]
        private void DisposeDialog()
        {
            if (SelectedDialog is null)
            {
                return;
            }

            SelectedDialog.Module.Dispose();
            Dialogs.Remove(SelectedDialog);
        }
    }
}
