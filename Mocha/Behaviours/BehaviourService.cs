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
    public class BehaviourService : IBehaviourService
    {
        private readonly Dictionary<string, object> _behaviours = new Dictionary<string, object>();

        /// <summary>
        /// Dictionary containing registered behaviours.
        /// </summary>
        public IReadOnlyDictionary<string, object> RegisteredBehaviours => new ReadOnlyDictionary<string, object>(_behaviours);

        /// <inheritdoc/>
        public void Record(string id, Action behaviour)
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

        /// <inheritdoc/>
        public void Record<TParam>(string id, Action<TParam> behaviour)
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

        /// <inheritdoc/>
        public void Record<TParam, TResult>(string id, Func<TParam, TResult> behaviour)
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

        /// <inheritdoc/>
        public IBehaviour Recall(string id)
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

        /// <inheritdoc/>
        public IBehaviour<TParam> Recall<TParam>(string id)
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

        /// <inheritdoc/>
        public IBehaviour<TParam, TResult> Recall<TParam, TResult>(string id)
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
