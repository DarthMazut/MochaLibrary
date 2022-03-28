using MochaCore.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class BlankPage2ViewModel : INavigatable
    {
        public BlankPage2ViewModel()
        {
            Navigator = new(this, NavigationServices.MainNavigationService);
        }

        public Navigator Navigator { get; }
    }
}
