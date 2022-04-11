using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.Utils
{
    public class NotifyDependencyPropertyEventArgs : EventArgs
    {
        public NotifyDependencyPropertyEventArgs(string dependencyPropertyName, object? value)
        {
            DependencyPropertyName = dependencyPropertyName;
            Value = value;
        }

        /// <summary>
        /// Specifies the name of dependency property that is to be changed.
        /// </summary>
        public string DependencyPropertyName { get; set; }

        /// <summary>
        /// Provides a value for dependency property to be changed or parameter value for command to be invoked.
        /// </summary>
        public object? Value { get; set; }

        public static NotifyDependencyPropertyEventArgs FromControlNotifierValue(ControlNotifierValue value)
        {
            return new NotifyDependencyPropertyEventArgs(value.PropertyName, value.Value);
        }
    }
}
