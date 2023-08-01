using Microsoft.UI.Xaml.Markup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels;

namespace WinUiApplication.Utils.Xaml
{
    public class MenuPagesExtension : MarkupExtension
    {
        protected override object ProvideValue()
        {
            return ViewModels.Pages.GetMenuPages();
        }
    }
}
