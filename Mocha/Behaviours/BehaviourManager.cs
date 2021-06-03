using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Mocha.Behaviours
{
    /// <summary>
    /// Allows for registration of technology-specific actions (behaviours)
    /// and later invocation of recorded behaviours within technology-independent modules.
    /// </summary>
    public static class BehaviourManager
    {
        private static readonly Dictionary<string, object> _behaviours = new Dictionary<string, object>();

        /// <summary>
        /// Dictionary containing registered behaviours.
        /// </summary>
        public static IReadOnlyDictionary<string, object> RegisteredBehaviours => new ReadOnlyDictionary<string, object>(_behaviours);

        /// <summary>
        /// Registers an action (behaviour).
        /// </summary>
        /// <param name="id">Identifier for registered behaviour.</param>
        /// <param name="behaviour">Action to be registered.</param>
        public static void Record(string id, Action behaviour)
        {
            if (_behaviours.ContainsKey(id))
            {
                throw new InvalidOperationException($"Id {id} was already used.");
            }
            else
            {
                _behaviours.Add(id, new Behaviour(behaviour));
            }
        }

        /// <summary>
        /// Registers an action (behaviour) with parameter.
        /// </summary>
        /// <typeparam name="TParam">Type of action parameter.</typeparam>
        /// <param name="id">Identifier for registered behaviour.</param>
        /// <param name="behaviour">Action to be registered.</param>
        public static void Record<TParam>(string id, Action<TParam> behaviour)
        {
            if (_behaviours.ContainsKey(id))
            {
                throw new InvalidOperationException($"Id {id} was already used.");
            }
            else
            {
                _behaviours.Add(id, new Behaviour<TParam>(behaviour));
            }
        }

        /// <summary>
        /// Registers an action (behaviour) with parameter which returns a value.
        /// </summary>
        /// <typeparam name="TParam">Type of action parameter.</typeparam>
        /// <typeparam name="TResult">Type of returning value.</typeparam>
        /// <param name="id">Identifier for registered behaviour.</param>
        /// <param name="behaviour">Function to be registered.</param>
        public static void Record<TParam, TResult>(string id, Func<TParam, TResult> behaviour)
        {
            if (_behaviours.ContainsKey(id))
            {
                throw new InvalidOperationException($"Id {id} was already used.");
            }
            else
            {
                _behaviours.Add(id, new Behaviour<TParam, TResult>(behaviour));
            }
        }

        /// <summary>
        /// Retrieves an <see cref="IBehaviour"/> object associated with givent ID.
        /// </summary>
        /// <param name="id">Identifier of requested behaviour.</param>
        public static IBehaviour Recall(string id)
        {
            if(_behaviours.ContainsKey(id))
            {
                if(_behaviours[id] is IBehaviour behaviour)
                {
                    return behaviour;
                }

                throw new InvalidCastException($"Type mismatch. Expected is {typeof(IBehaviour)}, actual was {_behaviours[id].GetType()}");
            }

            throw new KeyNotFoundException($"Behaviour {id} was never registered.");
        }

        /// <summary>
        /// Retrieves an <see cref="IBehaviour{T}"/> object associated with givent ID.
        /// </summary>
        /// <typeparam name="TParam">Parameter type of requested behaviour.</typeparam>
        /// <param name="id">Identifier of requested behaviour.</param>
        public static IBehaviour<TParam> Recall<TParam>(string id)
        {
            if (_behaviours.ContainsKey(id))
            {
                if (_behaviours[id] is IBehaviour<TParam> behaviour)
                {
                    return behaviour;
                }

                throw new InvalidCastException($"Type mismatch. Expected is {typeof(IBehaviour<TParam>)}, actual was {_behaviours[id].GetType()}");
            }

            throw new KeyNotFoundException($"Behaviour {id} was never registered.");
        }

        /// <summary>
        /// Retrieves an <see cref="IBehaviour{T, U}"/> object associated with givent ID.
        /// </summary>
        /// <typeparam name="TParam">Parameter type of requested behaviour.</typeparam>
        /// <typeparam name="TResult">Type returning by requested behaviour.</typeparam>
        /// <param name="id">Identifier of requested behaviour.</param>
        public static IBehaviour<TParam, TResult> Recall<TParam, TResult>(string id)
        {
            if (_behaviours.ContainsKey(id))
            {
                if (_behaviours[id] is IBehaviour<TParam, TResult> behaviour)
                {
                    return behaviour;
                }

                throw new InvalidCastException($"Type mismatch. Expected is {typeof(IBehaviour<TParam, TResult>)}, actual was {_behaviours[id].GetType()}");
            }

            throw new KeyNotFoundException($"Behaviour {id} was never registered.");
        }
    }
}
