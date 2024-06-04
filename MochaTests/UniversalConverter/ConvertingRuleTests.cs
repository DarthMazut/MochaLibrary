using MochaCore.Utils.Xaml.UniversalConverter;
using MochaCore.Utils.Xaml.UniversalConverter.CoreExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaTests.UniversalConverter
{
    public class ConvertingRuleTests
    {
        private readonly List<string> _items = new()
        {
            "some test string",
            "first second third",
            "some other string here"
        };

        [Fact]
        public void Test()
        {
            CoreRule rule = new()
            {
                Conditions = new List<CoreCondition>()
                {
                    new() { Projection = new CoreCollectionLookup() { Index = 1 } },
                    new() { Projection = new CoreStringTransform() { SplitBy = " " } },
                    new() { Projection = new CoreCollectionLookup() { Index = 1 } },
                    new() { Condition = new CoreNumberComparision() { IsGreaterThan = 3 } },
                    new() { Condition = new CoreNumberComparision() { IsLesserThan = 10 } }
                }
            };

            bool isMatch = rule.CheckValueMatch(_items);
            Assert.True(isMatch);
        }
    }
}
