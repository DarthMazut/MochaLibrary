using Microsoft.UI.Xaml.Markup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinUiApplicationX.Utils.Xaml
{
    public class MenuPagesExtension : MarkupExtension
    {
        protected override object ProvideValue() => ViewModelsX.Pages.GetMenuPages();
    }
}
