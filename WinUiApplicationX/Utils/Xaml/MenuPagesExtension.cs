using Microsoft.UI.Xaml.Markup;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModelsX;

namespace WinUiApplicationX.Utils.Xaml
{
    public class MenuPagesExtension : MarkupExtension
    {
        protected override object ProvideValue() => AppPages.GetMenuPages();
    }

    public class FooterMenuPagesExtension : MarkupExtension
    {
        protected override object ProvideValue() => new AppPage[]
        {
            AppPages.SettingsPage
        }.ToImmutableArray();
    }
}
