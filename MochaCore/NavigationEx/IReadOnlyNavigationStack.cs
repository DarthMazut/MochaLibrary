using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.NavigationEx
{
    public interface IReadOnlyNavigationStack<T>
    {
        /// <summary>
        /// Returns number of items currently contained in this stack.
        /// </summary>
        public int Count { get; }

        /// <summary>
        /// Indicates whther <see cref="CurrentIndex"/> points to the base item of this stack.
        /// </summary>
        public bool IsBottomIndex { get; }

        /// <summary>
        /// Indicates whether <see cref="CurrentIndex"/> points to the last added item to this stack.
        /// </summary>
        public bool IsTopIndex { get; }

        /// <summary>
        /// Returns index of current item.
        /// </summary>
        public int CurrentIndex { get; }

        /// <summary>
        /// Returns current item.
        /// </summary>
        public T CurrentItem { get; }

        /// <summary>
        /// Returns internal collection of this stack as <see cref="IReadOnlyList{T}"/>.
        /// </summary>
        public IReadOnlyList<T> InternalCollection { get; }
    }
}
