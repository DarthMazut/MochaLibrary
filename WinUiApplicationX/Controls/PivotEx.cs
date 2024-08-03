using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinUiApplicationX.Controls
{
    public class PivotEx : Pivot
    {
        public PivotEx()
        {
            this.DefaultStyleKey = typeof(PivotEx);
        }
    }

    public class PivotHeaderItemEx : PivotHeaderItem
    {
        public PivotHeaderItemEx()
        {
            this.DefaultStyleKey = typeof(PivotHeaderItemEx);
        }
    }
}
