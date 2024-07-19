using Microsoft.UI.Xaml.Markup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinUiApplicationX.Utils.Xaml
{
    public class EnumValuesExtension : MarkupExtension
    {
        public Type? Type { get; set; }

        protected override object ProvideValue() => Type?.IsEnum == true ? Enum.GetValues(Type) : Array.Empty<object>();
    }
}
