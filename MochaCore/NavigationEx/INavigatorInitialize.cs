using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.NavigationEx
{
    public interface INavigatorInitialize
    {
        public void Initialize(INavigationModule module, INavigationService navigationService);
    }
}
