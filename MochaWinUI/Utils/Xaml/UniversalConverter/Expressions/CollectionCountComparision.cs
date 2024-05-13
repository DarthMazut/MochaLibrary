using Microsoft.UI.Xaml;
using MochaCore.Utils.Xaml.UniversalConverter.CoreExpressions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaWinUI.Utils.Xaml.UniversalConverter
{
    public class CollectionCountComparision : NumberComparision
    {
        public override object? CalculateExpression(object? value) => new CoreCollectionCountComparision()
        {
            IsEqualTo = IsEqualTo,
            IsNotEqualTo = IsNotEqualTo,
            IsGraterOrEqualTo = IsGraterOrEqualTo,
            IsGreaterThan = IsGreaterThan,
            IsLesserOrEqualTo = IsLesserOrEqualTo,
            IsLesserThan = IsLesserThan
        }.CalculateExpression(value);

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
