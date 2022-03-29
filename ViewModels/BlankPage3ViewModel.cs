using MochaCore.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class BlankPage3ViewModel : INavigatable
    {
        public BlankPage3ViewModel()
        {
            Navigator = new(this, NavigationServices.MainNavigationService);
        }

        public Navigator Navigator { get; }
    }
}
