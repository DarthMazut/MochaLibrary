using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.DialogsEx
{
    /// <summary>
    /// Allows for configuration of related dialog.
    /// </summary>
    public class DialogProperties
    {
        /// <summary>
        /// Contains a set of dialog parameters.
        /// </summary>
        public Dictionary<string, object?> Parameters { get; set; } = new();

        /// <summary>
        /// Tries to fetch parameter corresponding to specified key from
        /// <see cref="Parameters"/> dictionary. Returns default value of
        /// expected type in case of type mismatch or no parameter was found.
        /// </summary>
        /// <typeparam name="T">Expected type of retrieving value.</typeparam>
        /// <param name="key">Parameter key.</param>
        public T? GetParameter<T>(string key)
        {
            if (Parameters.TryGetValue(key, out object? parameter))
            {
                if (parameter is T value)
                {
                    return value;
                }
            }

            return default;
        }

        /// <summary>
        /// Adds or updates specified parameter within <see cref="Parameters"/> dictionary.
        /// </summary>
        /// <param name="key">Parameter key.</param>
        /// <param name="value">Value to be added or updated.</param>
        public void SetParameter(string key, object? value)
        {
            if (Parameters.ContainsKey(key))
            {
                Parameters[key] = value;
            }
            else
            {
                Parameters.Add(key, value);
            }
        }
    }
}
