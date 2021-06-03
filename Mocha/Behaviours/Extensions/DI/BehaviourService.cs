using System;
using System.Collections.Generic;
using System.Text;

namespace Mocha.Behaviours.Extensions.DI
{
    /// <summary>
    /// Provides implementation for <see cref="IBehaviourService"/>.
    /// </summary>
    public class BehaviourService : IBehaviourService
    {
        /// <inheritdoc/>
        public IBehaviour Recall(string id)
        {
            return BehaviourManager.Recall(id);
        }

        /// <inheritdoc/>
        public IBehaviour<TParam> Recall<TParam>(string id)
        {
            return BehaviourManager.Recall<TParam>(id);
        }

        /// <inheritdoc/>
        public IBehaviour<TParam, TResult> Recall<TParam, TResult>(string id)
        {
            return BehaviourManager.Recall<TParam, TResult>(id);
        }
    }
}
