using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.Utils.Xaml.UniversalConverter.CoreExpressions
{
    public class CoreCollection : IConvertingExpression
    {
        public int? Index { get; set; }

        public CollectionOperation? Operation { get; set; }

        public bool IsConditionExpression => false;

        public object? CalculateExpression(object? value)
        {
            
        }
    }

    public enum CollectionOperation
    {
        Index,
        Count,
        Max,
        Min,
    }
}
