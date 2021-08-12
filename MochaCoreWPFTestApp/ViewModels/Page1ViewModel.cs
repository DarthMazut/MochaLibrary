using MochaCore.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCoreWPFTestApp.ViewModels
{
    public class Page1ViewModel : INavigatable
    {
        public Page1ViewModel()
        {
            Navigator = new Navigator(this, NavigationServices.MainNavigationService);
        }

        public Navigator Navigator { get; }
    }
}
