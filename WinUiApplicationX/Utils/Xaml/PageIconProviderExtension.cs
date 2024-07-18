using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Markup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModelsX;

namespace WinUiApplicationX.Utils.Xaml
{
    public class PageIconProviderExtension : MarkupExtension
    {
        public string PageId { get; set; } = string.Empty;

        public PageIconProviderExtension() { }

        public PageIconProviderExtension(string pageId)
        {
            PageId = pageId;
        }

        protected override object ProvideValue() => AppPages.GetById(PageId).Glyph;
    }
}
