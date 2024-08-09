using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
        [ObservableProperty]
        private ObservableCollection<SystemDialog> _dialogs = [];

        [ObservableProperty]
        private SystemDialog? _selectedDialog;

        [RelayCommand]
        private void CreateDialog()
        {
            Dialogs.Add(SystemDialog.FromModule(AppDialogs.SystemSaveDialog.Module));
        }

        [RelayCommand]
        private void DisposeDialog(SystemDialog dialog)
        {
            dialog.Module.Dispose();
            Dialogs.Remove(dialog);
        }
    }
}
