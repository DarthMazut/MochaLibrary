using MochaCore.Utils.Xaml.UniversalConverter.CoreExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaTests.UniversalConverter
{
    public class StringInterpolationExpressionTests
    {
        private static readonly List<string> _values = new()
        {
            "Value 1",
            "Value 2",
            "Value 3"
        };

        [Fact]
        public void Test()
        {
            CoreStringInterpolation stringInterpolation = new()
            {
                String = "This test {0} test test {1} test"
            };

            object? result = stringInterpolation.CalculateExpression(_values);

            Assert.Equal("This test Value 1 test test Value 2 test", result);  
        }

        [Fact]
        public void Test2()
        {
            CoreStringInterpolation stringInterpolation = new()
            {
                String = "This test {0} test test"
            };

            object? result = stringInterpolation.CalculateExpression(20d);

            Assert.Equal("This test 20 test test", result);
        }
    }
}
