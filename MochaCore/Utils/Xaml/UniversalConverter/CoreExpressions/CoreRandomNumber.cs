using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.Utils.Xaml.UniversalConverter.CoreExpressions
{
    public class CoreRandomNumber : IConvertingExpression
    {
        public int? LowerBound { get; init; }

        public int? UpperBound { get; init; }

        public bool IsConditionExpression => false;

        public object? CalculateExpression(object? value)
        {
            int lowerBound = 0;
            int upperBound = 1;

            if (LowerBound is not null && UpperBound is not null)
            {
                lowerBound = LowerBound.Value;
                upperBound = UpperBound.Value;
            }
            else if (int.TryParse(value?.ToString(), out int valueInt))
            {
                if (UpperBound is not null)
                {
                    lowerBound = valueInt;
                    upperBound = UpperBound.Value;
                }
                else if (LowerBound is not null)
                {
                    lowerBound = LowerBound.Value;
                    upperBound = valueInt;
                }
                else
                {
                    upperBound = valueInt;
                }
            }
            else
            {
                lowerBound = LowerBound ?? lowerBound;
                upperBound = UpperBound ?? upperBound;
            }

            return Random.Shared.Next(lowerBound, upperBound + 1);
        }
    }
}
