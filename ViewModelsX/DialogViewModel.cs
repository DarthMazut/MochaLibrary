using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MochaCore.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModelsX
{
    public partial class DialogViewModel : ObservableObject, ICustomDialog<MyDialogProperties>
    {
        public ICustomDialogControl<MyDialogProperties> DialogControl { get; } = new CustomDialogControl<MyDialogProperties>();

        public DialogViewModel()
        {
            DialogControl.Closing += (s, e) =>
            {
                
            };
        }

        [RelayCommand]
        private void Primary()
        {
        }
    }
}
