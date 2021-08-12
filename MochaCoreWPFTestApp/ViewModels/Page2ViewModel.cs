using MochaCore.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCoreWPFTestApp.ViewModels
{
    public class Page2ViewModel : INavigatable
    {
        public Page2ViewModel()
        {
            Navigator = new Navigator(this, NavigationServices.MainNavigationService);
        }

        public Navigator Navigator { get; }
    }
}
