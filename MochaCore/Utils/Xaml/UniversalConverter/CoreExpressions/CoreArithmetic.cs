using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.Utils.Xaml.UniversalConverter.CoreExpressions
{
    public class CoreArithmetic : IConvertingExpression
    {
        public double? Add { get; init; }

        public double? Substract { get; init; }

        public double? Multiply { get; init; }

        public double? Divide { get; init; }

        public double? Modulo { get; init; }

        public bool IsConditionExpression => false;

        public object? CalculateExpression(object? value)
        {
            if(double.TryParse(value.ToString(), out double doubleValue))
            {
                if (Multiply is not null)
                {
                    doubleValue *= Multiply.Value;
                }

                if (Divide is not null)
                {
                    doubleValue /= Divide.Value;
                }

                if (Modulo is not null)
                {
                    doubleValue %= Modulo.Value;
                }

                if (Add is not null)
                {
                    doubleValue += Add.Value;
                }

                if (Substract is not null)
                {
                    doubleValue -= Substract.Value;
                }

                return doubleValue;
            }

            throw new ArgumentException("Only numeric value supports arithmetic operations.");
        }
    }
}
