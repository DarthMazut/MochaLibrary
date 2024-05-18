using MochaCore.Utils.Xaml.UniversalConverter.CoreExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MochaTests.UniversalConverter
{
    public class ArithmeticExpressionTests
    {
        private readonly List<object> _testCollection = new()
        {
            new object(),
            new object(),
            new object(),
        };

        [Theory]
        [InlineData(3d, 2d, 1d, 4d, 1d, 0d, 3d)]
        [InlineData(10d, 1d, 2d, 2d, 1d, 2d, 0d)]
        [InlineData(100d, 2d, 20d, 10d, 0d, 4d, -4d)]
        [InlineData(4d, null, null, null, null, null, 4d)]
        [InlineData("2", 2d, 4d, null, null, null, 1d)]
        public void ShouldReturnProperValue(object? initial, double? multiplayer, double? divide, double? modulo, double? add, double? substract, double? result)
        {
            CoreArithmetic underTest = new CoreArithmetic()
            {
                Multiply = multiplayer,
                Divide = divide,
                Modulo = modulo,
                Add = add,
                Substract = substract
            };

            Assert.Equal(result, underTest.CalculateExpression(initial));
        }

        [Fact]
        public void ShouldReturnProperValue_WhenCollectionIsProvided()
        {
            CoreArithmetic underTest = new CoreArithmetic()
            {
                Multiply = 2,
                Add = 1
            };

            object? result = underTest.CalculateExpression(_testCollection);
            Assert.Equal(7d, result);
        }

        [Fact]
        public void ShouldThrow_WhenNoNumberGiven()
        {
            Assert.Throws<ArgumentException>(() => new CoreArithmetic().CalculateExpression(new object()));
        }
    }
}
