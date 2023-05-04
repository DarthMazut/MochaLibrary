using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.NavigationEx
{
    public interface INavigationStackItem
    {
        public INavigationModule Module { get; }

        public bool IsModalOrigin { get; }
    }
}
