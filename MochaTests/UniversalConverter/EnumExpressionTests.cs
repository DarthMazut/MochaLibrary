using MochaCore.Utils.Xaml.UniversalConverter.CoreExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaTests.UniversalConverter
{
    public class EnumExpressionTests
    {
        [Fact]
        public void ShouldReturnEnumValue_WhenTypeAndValueNameGiven()
        {
            CoreEnum coreEnum = new()
            {
                Type = typeof(TestEnum),
                Value = nameof(TestEnum.Enumeration2)
            };

            Assert.Equal(
                TestEnum.Enumeration2,
                coreEnum.CalculateExpression(default));
        }

        [Fact]
        public void ShouldReturnEnumValue_WhenTypeAndValueNumberGiven()
        {
            CoreEnum coreEnum = new()
            {
                Type = typeof(TestEnum),
                Value = "5"
            };

            Assert.Equal(
                TestEnum.Enumeration2,
                coreEnum.CalculateExpression(default));
        }

        [Fact]
        public void ShouldReturnEnumValue_WhenTypeNameAndValueNameGiven()
        {
            CoreEnum coreEnum = new()
            {
                TypeName = typeof(TestEnum).AssemblyQualifiedName,
                Value = nameof(TestEnum.Enumeration2)
            };

            Assert.Equal(
                TestEnum.Enumeration2,
                coreEnum.CalculateExpression(default));
        }

        [Fact]
        public void ShouldReturnEnumValue_WhenTypeNameAndValueNumberGiven()
        {
            CoreEnum coreEnum = new()
            {
                TypeName = typeof(TestEnum).AssemblyQualifiedName,
                Value = "5"
            };

            Assert.Equal(
                TestEnum.Enumeration2,
                coreEnum.CalculateExpression(default));
        }

        [Fact]
        public void ShouldReturnEnumValue_WhenTypeGivenAndProcessingValueIsName()
        {
            CoreEnum coreEnum = new()
            {
                Type = typeof(TestEnum),
            };

            Assert.Equal(
                TestEnum.Enumeration2,
                coreEnum.CalculateExpression(nameof(TestEnum.Enumeration2)));
        }

        [Fact]
        public void ShouldReturnEnumValue_WhenTypeGivenAndProcessingValueIsNumber()
        {
            CoreEnum coreEnum = new()
            {
                Type = typeof(TestEnum),
            };

            Assert.Equal(
                TestEnum.Enumeration2,
                coreEnum.CalculateExpression(5));
        }

        [Fact]
        public void ShouldReturnEnumValue_WhenTypeGivenAndProcessingValueIsEnumItself()
        {
            CoreEnum coreEnum = new()
            {
                Type = typeof(TestEnum),
            };

            Assert.Equal(
                TestEnum.Enumeration2,
                coreEnum.CalculateExpression(TestEnum.Enumeration2));
        }

        [Fact]
        public void ShouldThrow_WhenTypeNotSpecified()
        {
            CoreEnum coreEnum = new()
            {
                Value = TestEnum.Enumeration2
            };

            Assert.Throws<ArgumentException>(() => coreEnum.CalculateExpression(TestEnum.Enumeration2));
        }

        [Fact]
        public void ShouldThrow_WhenProcessingValueIsNotEnumParsable()
        {
            CoreEnum coreEnum = new()
            {
                Type = typeof(TestEnum),
                Value = "Test"
            };

            Assert.Throws<ArgumentException>(() => coreEnum.CalculateExpression(TestEnum.Enumeration2));
        }

        private enum TestEnum
        {
            Enumeration1 = 1,
            Enumeration2 = 5,
            Enumeration3 = 10
        }
    }
}
