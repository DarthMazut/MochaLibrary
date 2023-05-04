using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.NavigationEx
{
    /// <summary>
    /// Represents a navigation stack that allows for forward and backward navigation through a collection of items.
    /// </summary>
    /// <typeparam name="T">The type of items in the navigation stack.</typeparam>
    public class NavigationStack<T> : IReadOnlyNavigationStack<T>
    {
        private readonly List<T> _internalCollection;
        private int _currentIndex = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationStack{T}"/> class
        /// with specified base item. The base item is always on the bottom of the stack
        /// and cannot be removed.
        /// </summary>
        /// <param name="baseItem">Base item of creating instance.</param>
        public NavigationStack(T baseItem)
        {
            _internalCollection = new List<T> { baseItem };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationStack{T}"/> class
        /// with predefined items.
        /// </summary>
        /// <param name="items">Initial items of creating instance.</param>
        public NavigationStack(IEnumerable<T> items)
        {
            _internalCollection = new List<T>(items);
            _currentIndex = items.Count() - 1;
        }

        /// <summary>
        /// Determines whether items removed from this stack should be disposed
        /// (if they're <see cref="IDisposable"/>).
        /// </summary>
        public bool DisposeOnRemove { get; set; } = false;

        /// <summary>
        /// Returns number of items currently contained in this stack.
        /// </summary>
        public int Count => _internalCollection.Count;

        /// <summary>
        /// Indicates whther <see cref="CurrentIndex"/> points to the base item of this stack.
        /// </summary>
        public bool IsBottomIndex => _currentIndex == 0;

        /// <summary>
        /// Indicates whether <see cref="CurrentIndex"/> points to the last added item to this stack.
        /// </summary>
        public bool IsTopIndex => _currentIndex == _internalCollection.Count - 1;

        /// <summary>
        /// Returns index of current item.
        /// </summary>
        public int CurrentIndex => _currentIndex;

        /// <summary>
        /// Returns current item.
        /// </summary>
        public T CurrentItem => _internalCollection[_currentIndex];

        /// <summary>
        /// Returns internal collection of this stack as <see cref="IReadOnlyList{T}"/>.
        /// </summary>
        public IReadOnlyList<T> InternalCollection => _internalCollection.AsReadOnly();

        /// <summary>
        /// Adds a new item onto the stack. If the current index is not the top index, 
        /// all items above the current index will be removed. If removed items are <see cref="IDisposable"/>
        /// and the <see cref="DisposeOnRemove"/> is set to <see langword="true"/> these items will receive
        /// a call to theirs <see cref="IDisposable"/> implementation.
        /// </summary>
        /// <param name="newItem">Item to be pushed onto the stack.</param>
        public void Push(T newItem)
        {
            if (!IsTopIndex)
            {
                for (int i = _internalCollection.Count - 1; i > _currentIndex; i--)
                {
                    HandleItemDisposal(_internalCollection[i]);
                    _internalCollection.RemoveAt(i);
                }
            }

            _internalCollection.Add(newItem);
            _currentIndex = _internalCollection.Count - 1;
        }

        public T Pop() => Pop(1)[0];

        /// <summary>
        /// Removes the specified number of items from the stack starting from the current item and returns the 
        /// removed items as an <see cref="IReadOnlyList{T}"/>. If the current index does not point to the top 
        /// of the stack, the elements above the current index are also removed and not included in the returned 
        /// list. If removed items are <see cref="IDisposable"/> and the <see cref="DisposeOnRemove"/> is set to 
        /// <see langword="true"/> these items will receive a call to theirs <see cref="IDisposable"/> implementation. 
        /// If the specified number of items cannot be removed from the stack, an <see cref="InvalidOperationException"/> is thrown.
        /// </summary>
        /// <param name="itemsCount">Number of items to be popped from the stack.</param>
        /// <exception cref="InvalidOperationException"></exception>
        public IReadOnlyList<T> Pop(int itemsCount)
        {
            if (itemsCount >= Count)
            {
                throw new InvalidOperationException($"Cannot remove base item from {nameof(NavigationStack<T>)}.");
            }

            List<T> removedItems = new();
            for (int i = _internalCollection.Count - 1; i > _currentIndex - itemsCount; i--)
            {
                removedItems.Add(_internalCollection[i]);
                HandleItemDisposal(_internalCollection[i]);
                _internalCollection.RemoveAt(i);
            }

            _currentIndex -= itemsCount;
            return removedItems.TakeLast(itemsCount).ToList().AsReadOnly();
        }

        public bool TryPop(out T poppedItem)
        {
            throw new NotImplementedException();
        }

        public bool TryPop(int itemsCount, out IReadOnlyList<T> poppedItems)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Overwrites the current item in the stack with a new item. If the 
        /// <see cref="DisposeOnRemove"/> property is <see langword="true"/>, this method
        /// will dispose of the previous item.
        /// </summary>
        /// <param name="newItem">Item to overwrite current one.</param>
        public void OverwriteCurrent(T newItem)
        {
            HandleItemDisposal(_internalCollection[_currentIndex]);
            _internalCollection[_currentIndex] = newItem;
        }

        public void MoveBack()
        {
            if (IsBottomIndex)
            {
                throw new InvalidOperationException("Cannot move back from the bottom index.");
            }

            _currentIndex--;
        }

        public bool TryMoveBack()
        {
            if (IsBottomIndex)
            {
                return false;
            }

            MoveBack();
            return true;
        }

        public void MoveBack(int step)
        {
            if (_currentIndex < step)
            {
                throw new InvalidOperationException("Cannot move back from the bottom index.");
            }

            _currentIndex -= step;
        }

        public bool TryMoveBack(int step)
        {
            if (_currentIndex < step)
            {
                return false;
            }

            MoveBack(step);
            return true;
        }

        public void MoveForward()
        {
            if (_currentIndex >= _internalCollection.Count - 1)
            {
                throw new InvalidOperationException("Cannot move forward from the top index.");
            }

            _currentIndex++;
        }

        public bool TryMoveForward()
        {
            if (IsTopIndex)
            {
                return false;
            }

            MoveForward();
            return true;
        }

        public void MoveForward(int step)
        {
            if (_currentIndex + step >= _internalCollection.Count)
            {
                throw new InvalidOperationException("Cannot move forward from the top index.");
            }

            _currentIndex += step;
        }

        public bool TryMoveForward(int step)
        {
            if (_currentIndex + step >= _internalCollection.Count)
            {
                return false;
            }

            MoveForward(step);
            return true;
        }

        public T? PeekBack(int step)
        {
            if (step > _currentIndex)
            {
                return default;
            }

            return _internalCollection[_currentIndex - step];
        }

        public T? PeekForward(int step)
        {
            if (step > Count - _currentIndex - 1)
            {
                return default;
            }

            return _internalCollection[_currentIndex + step];
        }


        // może tutaj z minusem?
        public T? Peek(NavigationType navigationType, int step)
        {
            return navigationType switch
            {
                NavigationType.Push => throw new ArgumentException($"Cannot use {nameof(Peek)} method if {nameof(NavigationType)} is {NavigationType.Push}", nameof(navigationType)),
                NavigationType.Back => PeekBack(step),
                NavigationType.Forward => PeekForward(step),
                _ => throw new NotImplementedException()
            };
        }

        /// <summary>
        /// Removes all items from this stack except the base item. If removed items are
        /// <see cref="IDisposable"/> and <see cref="DisposeOnRemove"/> is set to <see langword="true"/>
        /// removed items will receive call to their <see cref="IDisposable"/> implementations.
        /// </summary>
        public void Clear()
        {
            for (int i = _internalCollection.Count - 1; i > 0; i--)
            {
                HandleItemDisposal(_internalCollection[i]);
                _internalCollection.RemoveAt(i);
            }

            _currentIndex = 0;
        }

        public IReadOnlyNavigationStack<T> ToReadOnlyStack()
            => new ReadOnlyNavigationStack<T>(InternalCollection.ToArray(), CurrentIndex);

        public IReadOnlyNavigationStack<TOutput> ToReadOnlyStack<TOutput>(Func<T, TOutput> translationFunc)
            => new ReadOnlyNavigationStack<TOutput>(InternalCollection
                .Select(i => translationFunc(i)).ToArray(),
                CurrentIndex);

        private void HandleItemDisposal(T item)
        {
            if (DisposeOnRemove && item is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }

}
