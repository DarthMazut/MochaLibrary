using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MochaCore.Dialogs;
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

        [ObservableProperty]
        private ObservableCollection<SystemDialog> _dialogs = [];

        [ObservableProperty]
        private SystemDialog? _selectedDialog;

        [ObservableProperty]
        private bool _isPaneOpen;
        

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
