using Microsoft.UI.Xaml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinUiApplication.Converters.UniversalConverter
{
    public class CollectionCountComparision : NumberComparision
    {
        public override object? CalculateExpression(object? value)
        {
            int? count = null;
            if (value is ICollection collection)
            {
                count = collection.Count;
                
            }

            if (value is IEnumerable<object?> enumerable)
            {
                count = enumerable.Count();
            }

            if (count is not null)
            {
                return base.CalculateExpression(count);
            }
            
            throw new ArgumentException("Cannot resolve value as collection.");
        }

        protected override object ProvideValue(IXamlServiceProvider serviceProvider)
        {
            return new CollectionCountComparision()
            {
                IsEqualTo = IsEqualTo,
                IsNotEqualTo = IsNotEqualTo,
                IsGraterOrEqualTo = IsGraterOrEqualTo,
                IsGreaterThan = IsGreaterThan,
                IsLesserOrEqualTo = IsLesserOrEqualTo,
                IsLesserThan = IsLesserThan
            };
        }
    }
}
