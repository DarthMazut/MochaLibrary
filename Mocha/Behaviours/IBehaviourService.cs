using System;
using System.Collections.Generic;
using System.Text;

namespace Mocha.Behaviours
{
    /// <summary>
    /// Allows for registration of technology-specific actions (behaviours)
    /// and later invocation of recorded behaviours within technology-independent modules.
    /// </summary>
    public interface IBehaviourService
    {
        /// <summary>
        /// Registers an action (behaviour).
        /// </summary>
        /// <param name="id">Identifier for registered behaviour.</param>
        /// <param name="behaviour">Action to be registered.</param>
        void Record(string id, Action behaviour);

        /// <summary>
        /// Registers an action (behaviour) with parameter.
        /// </summary>
        /// <typeparam name="TParam">Type of action parameter.</typeparam>
        /// <param name="id">Identifier for registered behaviour.</param>
        /// <param name="behaviour">Action to be registered.</param>
        void Record<TParam>(string id, Action<TParam> behaviour);

        /// <summary>
        /// Registers an action (behaviour) with parameter which returns a value.
        /// </summary>
        /// <typeparam name="TParam">Type of action parameter.</typeparam>
        /// <typeparam name="TResult">Type of returning value.</typeparam>
        /// <param name="id">Identifier for registered behaviour.</param>
        /// <param name="behaviour">Function to be registered.</param>
        void Record<TParam, TResult>(string id, Func<TParam, TResult> behaviour);

        /// <summary>
        /// Retrieves an <see cref="IBehaviour"/> object associated with givent ID.
        /// </summary>
        /// <param name="id">Identifier of requested behaviour.</param>
        IBehaviour Recall(string id);

        /// <summary>
        /// Retrieves an <see cref="IBehaviour{T}"/> object associated with givent ID.
        /// </summary>
        /// <typeparam name="TParam">Parameter type of requested behaviour.</typeparam>
        /// <param name="id">Identifier of requested behaviour.</param>
        IBehaviour<TParam> Recall<TParam>(string id);

        /// <summary>
        /// Retrieves an <see cref="IBehaviour{T, U}"/> object associated with givent ID.
        /// </summary>
        /// <typeparam name="TParam">Parameter type of requested behaviour.</typeparam>
        /// <typeparam name="TResult">Type returning by requested behaviour.</typeparam>
        /// <param name="id">Identifier of requested behaviour.</param>
        IBehaviour<TParam, TResult> Recall<TParam, TResult>(string id);
    }
}
