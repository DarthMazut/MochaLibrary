﻿using CommunityToolkit.Mvvm.ComponentModel;
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
            SystemDialog createdDialog = SelectedType switch
            {
                SystemDialogType.SaveDialog => SystemDialog.FromModule(AppDialogs.SystemSaveDialog.Module),
                SystemDialogType.OpenDialog => SystemDialog.FromModule(AppDialogs.SystemOpenDialog.Module),
                SystemDialogType.BrowseDialog => SystemDialog.FromModule(AppDialogs.SystemBrowseDialog.Module),
                _ => throw new NotImplementedException(),
            };
            
            if (!string.IsNullOrWhiteSpace(Name))
            {
                createdDialog.Name = Name;
            }

            DialogControl.Properties.CreatedDialog = createdDialog;
        }
    }
}
