using MochaCore.Utils.Xaml.UniversalConverter.CoreExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaTests.UniversalConverter
{
    public class DoubleExpressionTests
    {
        [Fact]
        public void ShouldUseValue_WhenProvided()
        {
            CoreDouble coreDouble = new()
            {
                Value = 5d
            };

            Assert.Equal(5d, coreDouble.CalculateExpression("6"));
        }

        [Fact]
        public void ShouldConvertProcessingValue_WhenNoValueProvided()
        {
            CoreDouble coreDouble = new();

            Assert.Equal(7d, coreDouble.CalculateExpression("07"));
        }

        [Fact]
        public void ShouldThrow_WhenNoValueProvidedAndProcessingValueIsNotParsable()
        {
            CoreDouble coreDouble = new();

            Assert.Throws<ArgumentException>(() => coreDouble.CalculateExpression(new object()));
        }
    }
}
