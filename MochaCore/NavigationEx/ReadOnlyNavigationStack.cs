using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.NavigationEx
{
    public class ReadOnlyNavigationStack<T> : IReadOnlyNavigationStack<T>
    {
        private readonly ICollection<T> _source;
        private readonly int _index;

        public ReadOnlyNavigationStack(ICollection<T> source, int currentIndex)
        {
            if (currentIndex < 0 || currentIndex > source.Count - 1)
            {
                throw new ArgumentOutOfRangeException(nameof(currentIndex));
            }

            _source = source ?? throw new ArgumentNullException(nameof(source));
            _index = currentIndex;
        }

        /// <inheritdoc/>
        public int Count => _source.Count;

        /// <inheritdoc/>
        public bool IsBottomIndex => _index == 0;

        /// <inheritdoc/>
        public bool IsTopIndex => _index == _source.Count - 1;

        /// <inheritdoc/>
        public int CurrentIndex => _index;

        /// <inheritdoc/>
        public T CurrentItem => _source.ElementAt(_index);

        /// <inheritdoc/>
        public IReadOnlyList<T> InternalCollection => new List<T>(_source).AsReadOnly();
    }
}
