using MochaCore.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class SettingsPageViewModel : INavigatable
    {
        public SettingsPageViewModel()
        {
            Navigator = new(this, NavigationServices.MainNavigationService);
        }
        public Navigator Navigator { get; }
    }
}
