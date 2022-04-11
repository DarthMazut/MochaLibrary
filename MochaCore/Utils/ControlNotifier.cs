using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.Utils
{
    /// <summary>
    /// Provides API for changing values of dependency properties and invoking commands
    /// of user control from its data context.
    /// </summary>
    public class ControlNotifier : INotifyPropertyChanged
    {
        private ControlNotifierValue _value = new(string.Empty, null);

        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Proeprty to be bind with *DependencyPropertyNotifier* instance.
        /// </summary>
        public ControlNotifierValue Binding 
        {
            get => _value;
            set
            {
                if (value != _value)
                {
                    _value = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Binding)));
                }
            }
        }

        public void SetDependencyPropertyValue(string propertyName, object? newValue)
        {
            Binding = new ControlNotifierValue(propertyName, newValue);
        }

        public void InvokeCommand(string commandName, object? parameter)
        {
            Binding = new ControlNotifierValue(commandName, parameter);
        }

    }

    /// <summary>
    /// Internal data type of <see cref="ControlNotifier"/> class.
    /// </summary>
    public class ControlNotifierValue
    {
        public ControlNotifierValue(string propertyName, object? value)
        {
            PropertyName = propertyName;
            Value = value;
        }

        public string PropertyName { get; }

        public object? Value { get; }
    }
}
