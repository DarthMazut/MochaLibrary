using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.Utils
{
    public class AsyncPropertyChangedEventArgs<T> : EventArgs
    {
        private bool _isTimedOut;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncPropertyChangedEventArgs{T}"/> class.
        /// </summary>
        /// <param name="host">Owner of <see cref="AsyncProperty{T}"/> for which this object is being initialized.</param>
        /// <param name="previousValue">Value before change.</param>
        /// <param name="newValue">Value after change.</param>
        public AsyncPropertyChangedEventArgs(INotifyPropertyChanged host, T? previousValue, T? newValue)
        {
            Host = host;
            PreviousValue = previousValue;
            NewValue = newValue;
        }

        /// <summary>
        /// Value before change.
        /// </summary>
        public T? PreviousValue { get; }

        /// <summary>
        /// Value after change.
        /// </summary>
        public T? NewValue { get; }

        /// <summary>
        /// Owner of <see cref="AsyncProperty{T}"/> for which this object was created.
        /// </summary>
        public INotifyPropertyChanged Host { get; }

        /// <summary>
        /// Specifies whether <see cref="AsyncProperty{T}.PropertyChangedOperationTimeout"/>
        /// has been reached.
        /// </summary>
        public bool IsTimedOut => _isTimedOut;

        internal void MarkTimedOut()
        {
            _isTimedOut = true;
        }
    }
}
