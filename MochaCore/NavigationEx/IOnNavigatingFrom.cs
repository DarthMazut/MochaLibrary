using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MochaCore.NavigationEx
{
    public interface IOnNavigatingFrom
    {
        public void OnNavigatingFrom(OnNavigatingFromEventArgs e);
    }
}
