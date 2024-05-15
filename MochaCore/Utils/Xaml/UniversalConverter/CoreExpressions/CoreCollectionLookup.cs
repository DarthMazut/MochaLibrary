using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.Utils.Xaml.UniversalConverter.CoreExpressions
{
    public class CoreCollectionLookup : IConvertingExpression
    {
        public int? Index { get; init; }

        public CollectionOperation? Operation { get; init; }

        public bool IsConditionExpression => false;

        public object? CalculateExpression(object? value)
        {
            if (Index is not null)
            {
                if (value is IList list)
                {
                    return list[Index.Value];
                }

                throw new ArgumentException("Collection does not expose indexer.");
            }

            return Operation switch
            {
                null => ResolveCount(value),
                CollectionOperation.Count => ResolveCount(value),
                CollectionOperation.First => (value as IEnumerable)?.Cast<object?>().FirstOrDefault() 
                    ?? throw new ArgumentException("Cannot acces first element. Make sure provided value is valid collection."),
                CollectionOperation.Last => (value as IEnumerable)?.Cast<object?>().LastOrDefault()
                    ?? throw new ArgumentException("Cannot acces last element. Make sure provided value is valid collection."),
                CollectionOperation.Max => (value as IEnumerable)?.Cast<object?>().Max()
                    ?? throw new ArgumentException("Cannot get maximum element. Make sure provided value is valid collection."),
                CollectionOperation.Min => (value as IEnumerable)?.Cast<object?>().Min()
                    ?? throw new ArgumentException("Cannot get minimum element. Make sure provided value is valid collection."),
                _ => throw new NotImplementedException(),
            };
        }

        private static int ResolveCount(object? value)
        {
            if (value is ICollection collection)
            {
                return collection.Count;
            }

            if (value is IEnumerable enumerable)
            {
                return enumerable.Cast<object?>().Count();
            }

            throw new ArgumentException("Couldn't determine collection length.");
        }
    }

    public enum CollectionOperation
    {
        Count,
        First,
        Last,
        Max,
        Min,
    }
}
