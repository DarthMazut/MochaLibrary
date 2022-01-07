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
        public AsyncPropertyChangedEventArgs(INotifyPropertyChanged host, T? previousValue, T? newValue)
        {
            Host = host;
            PreviousValue = previousValue;
            NewValue = newValue;
        }

        public T? PreviousValue { get; }

        public T? NewValue { get; }

        public INotifyPropertyChanged Host { get; }
    }
}
