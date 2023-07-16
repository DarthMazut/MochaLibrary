using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MochaCore.NavigationEx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavigationTest.Pages.ModalPage
{
    public partial class ModalPageViewModel : ObservableObject, INavigatable
    {
        public INavigator Navigator { get; } = MochaCore.NavigationEx.Navigator.Create();

        [RelayCommand]
        private async Task NavigateModal()
        {
            string x = "";
            string? returnValue = (await Navigator.NavigateModalAsync("InnerModalPage")).Data as string;
            x = "xyz";
        }
    }
}
