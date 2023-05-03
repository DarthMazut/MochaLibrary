using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MochaCore.NavigationEx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavigationTest.Pages.InnerModalPage
{
    public partial class InnerModalPageViewModel : ObservableObject, INavigatable
    {
        public Navigator Navigator { get; } = new();

        [RelayCommand]
        private Task ReturnModal()
        {
            return Navigator.ReturnModal("Hello 😀");
        }
    }
}
