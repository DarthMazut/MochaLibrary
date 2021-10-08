using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace MochaCore.Utils
{
    /// <summary>
    /// Simplifies implementation of <see cref="INotifyPropertyChanged"/> pattern.
    /// </summary>
    public class PropertyNotifier
    {
        private readonly Action<PropertyChangedEventArgs>? _notifyPropertyChange;
        private readonly INotifyPropertyChanged? _observable;

        /// <summary>
        /// Initializes a new instance of <see cref="PropertyNotifier"/> class.
        /// </summary>
        /// <param name="notifyPropertyChange">Delegate which invokes <see cref="INotifyPropertyChanged"/> event.</param>
        public PropertyNotifier(Action<PropertyChangedEventArgs> notifyPropertyChange)
        {
            _notifyPropertyChange = notifyPropertyChange;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="PropertyNotifier"/> class.
        /// </summary>
        /// <param name="observable">Observable object which implements <see cref="INotifyPropertyChanged"/> interface.</param>
        public PropertyNotifier(INotifyPropertyChanged observable)
        {
            _observable = observable;
        }

        /// <summary>
        /// Attempts to change value of specified property and invokes <see cref="INotifyPropertyChanged"/> 
        /// event if value was actually changed. Returns <see langword="true"/> if event was invoked, 
        /// otherwise <see langword="false"/>.
        /// </summary>
        /// <typeparam name="T">Type of property.</typeparam>
        /// <param name="newValue">New value for property.</param>
        /// <param name="backingField">Reference to changing property backing field.</param>
        /// <param name="propertyName">Name of property being changed.</param>
        public bool ChangeAndNotify<T>(T newValue, ref T backingField, [CallerMemberName] string? propertyName = null)
        {
            if (!EqualityComparer<T>.Default.Equals(backingField, newValue))
            {
                backingField = newValue;
                
                if (_observable != null)
                {
                    NotifyPropertyChangedViaReflection(propertyName);
                }
                else
                {
                    _notifyPropertyChange?.Invoke(new PropertyChangedEventArgs(propertyName));
                }
                
                return true;
            }

            return false;
        }

        /// <summary>
        /// Attempts to change value of specified property and invokes <see cref="INotifyPropertyChanged"/> 
        /// event if value was actually changed. Returns <see langword="true"/> if event was invoked, 
        /// otherwise <see langword="false"/>.
        /// </summary>
        /// <typeparam name="T">Type of property.</typeparam>
        /// <param name="newValue">New value for property.</param>
        /// <param name="backingField">Reference to changing property backing field.</param>
        /// <param name="notifyPropertyChange">Delegate which invokes <see cref="INotifyPropertyChanged"/> event.</param>
        /// <param name="propertyName">Name of property being changed.</param>
        public static bool ChangeAndNotify<T>(T newValue, ref T backingField, Action<PropertyChangedEventArgs> notifyPropertyChange, [CallerMemberName] string? propertyName = null)
        {
            if (!EqualityComparer<T>.Default.Equals(backingField, newValue))
            {
                backingField = newValue;
                notifyPropertyChange?.Invoke(new PropertyChangedEventArgs(propertyName));
                return true;
            }

            return false;
        }

        private void NotifyPropertyChangedViaReflection(string? propertyName)
        {
            Type type = _observable!.GetType();
            FieldInfo? field = type.GetField(nameof(INotifyPropertyChanged.PropertyChanged), BindingFlags.NonPublic | BindingFlags.Instance);
            MulticastDelegate? eventDelegate = field?.GetValue(_observable) as MulticastDelegate;
            if (eventDelegate != null)
            {
                foreach (Delegate? eventHandler in eventDelegate.GetInvocationList())
                {
                    eventHandler.Method.Invoke(eventHandler.Target, new object[] { _observable, new PropertyChangedEventArgs(propertyName) });
                }
            }
        }
    }
}
