using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.Utils.Xaml.UniversalConverter.CoreExpressions
{
    /// <summary>
    /// Allows for arithmetic comparision of collection count.
    /// </summary>
    public class CoreCollectionCountComparision : CoreNumberComparision
    {
        /// <inheritdoc/>
        public override object? CalculateExpression(object? value)
        {
            int? count = null;
            if (value is ICollection collection)
            {
                count = collection.Count;
            }

            if (value is IEnumerable<object?> enumerable)
            {
                count = enumerable.Count();
            }

            if (count is not null)
            {
                return base.CalculateExpression(count);
            }

            throw new ArgumentException("Cannot resolve value as collection.");
        }
    }
}
