using System;
using System.Collections.Generic;
using System.Text;

namespace Mocha.Behaviours.Extensions.DI
{
    /// <summary>
    /// Allows for registration of technology-specific actions (behaviours)
    /// and later invocation of recorded behaviours within technology-independent modules.
    /// </summary>
    public interface IBehaviourService
    {
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
