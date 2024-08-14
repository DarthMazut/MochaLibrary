using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MochaCore.Dialogs;
using ModelX.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModelsX.Dialogs;

namespace ViewModelsX.Pages.Dialogs
{
    public partial class CreateDialogDialogViewModel : ObservableObject, ICustomDialog<CreateDialogDialogProperties>
    {
        public ICustomDialogControl<CreateDialogDialogProperties> DialogControl { get; } = new CustomDialogControl<CreateDialogDialogProperties>();

        [ObservableProperty]
        private SystemDialogType _selectedType;

        [ObservableProperty]
        private string _name = string.Empty;

        [RelayCommand]
        private void CreateDialog()
        {
            DialogControl.Properties.CreatedDialog = SelectedType switch
            {
                SystemDialogType.SaveDialog => new SystemDialog(AppDialogs.SystemSaveDialog.Module, Name),
                SystemDialogType.OpenDialog => new SystemDialog(AppDialogs.SystemOpenDialog.Module, Name),
                SystemDialogType.BrowseDialog => new SystemDialog(AppDialogs.SystemBrowseDialog.Module, Name),
                _ => throw new NotImplementedException(),
            };
        }
    }
}
