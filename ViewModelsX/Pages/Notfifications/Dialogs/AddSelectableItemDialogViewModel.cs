using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MochaCore.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModelsX.Pages.Notfifications.Dialogs
{
    public partial class AddSelectableItemDialogViewModel : ObservableObject, ICustomDialog<AddSelectableItemDialogProperties>
    {
        public ICustomDialogControl<AddSelectableItemDialogProperties> DialogControl { get; }
            = new CustomDialogControl<AddSelectableItemDialogProperties>();

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CanCreate))]
        private string? _key;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CanCreate))]
        private string? _name;

        public bool CanCreate => !string.IsNullOrWhiteSpace(Key) && !string.IsNullOrWhiteSpace(Name);

        [RelayCommand]
        private void AddItem() => DialogControl.Properties.CreatedItem = new(Key!, Name!);
    }
}
