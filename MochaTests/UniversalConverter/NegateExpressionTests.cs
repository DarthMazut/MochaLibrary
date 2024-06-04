using MochaCore.Utils.Xaml.UniversalConverter.CoreExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaTests.UniversalConverter
{
    public class NegateExpressionTests
    {
        private readonly CoreNegate _underTest = new();

        [Fact]
        public void ShouldNeagte_WhenBoolProvided()
        {
            // Arrange
            bool inputValue = true;

            // Act
            object? outputValue = _underTest.CalculateExpression(inputValue);

            // Assert
            Assert.Equal(false, outputValue);
        }

        [Fact]
        public void ShouldNeagte_WhenNumberProvided()
        {
            // Arrange
            int inputValue = -10;

            // Act
            object? outputValue = _underTest.CalculateExpression(inputValue);

            // Assert
            Assert.Equal(10d, outputValue);
        }

        [Fact]
        public void ShouldThrow_WhenNoBoolOrNumberProvided()
        {
            // Arrange
            string inputValue = "Test";

            // Act, Assert
            Assert.Throws<ArgumentException>(() => _underTest.CalculateExpression(inputValue));
        }
    }
}
