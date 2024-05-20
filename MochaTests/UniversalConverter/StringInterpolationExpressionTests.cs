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
        public void ShouldReturnFormatedString_WhenCollectionIsProvided()
        {
            CoreStringInterpolation stringInterpolation = new()
            {
                String = "A B {0} C D {1} E"
            };

            object? result = stringInterpolation.CalculateExpression(_values);

            Assert.Equal("A B Value 1 C D Value 2 E", result);  
        }

        [Fact]
        public void ShouldReturnFormatedString_WhenValueIsProvided()
        {
            CoreStringInterpolation stringInterpolation = new()
            {
                String = "A B {0} C D"
            };

            object? result = stringInterpolation.CalculateExpression(20d);

            Assert.Equal("A B 20 C D", result);
        }

        [Fact]
        public void ShouldThrow_WhenNoStringProvided()
        {
            CoreStringInterpolation stringInterpolation = new() { };

            Assert.Throws<ArgumentException>(() => stringInterpolation.CalculateExpression(default));
        }
    }
}
