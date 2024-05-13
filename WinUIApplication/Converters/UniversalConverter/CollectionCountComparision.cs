using Microsoft.UI.Xaml;
using MochaCore.Utils.Xaml.UniversalConverter.CoreExpressions;
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
        private readonly CoreCollectionCountComparision _coreExpression;

        public CollectionCountComparision()
        {
            _coreExpression = new CoreCollectionCountComparision()
            {
                IsEqualTo = IsEqualTo,
                IsNotEqualTo = IsNotEqualTo,
                IsGraterOrEqualTo = IsGraterOrEqualTo,
                IsGreaterThan = IsGreaterThan,
                IsLesserOrEqualTo = IsLesserOrEqualTo,
                IsLesserThan = IsLesserThan
            };
        }

        public override object? CalculateExpression(object? value) => _coreExpression.CalculateExpression(value);

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
