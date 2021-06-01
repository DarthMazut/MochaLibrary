using System;
using System.Collections.Generic;
using System.Text;

namespace Mocha.Behaviours
{
    /// <summary>
    /// Represent encapsulated action (behaviour) which can be executed by 
    /// technology-independent moduels.
    /// </summary>
    public interface IBehaviour
    {
        /// <summary>
        /// Executes encapsulated behaviour.
        /// </summary>
        void Execute();
    }

    /// <summary>
    /// Represent encapsulated action (behaviour) which can be executed by 
    /// technology-independent moduels.
    /// </summary>
    /// <typeparam name="TParam">Type of behaviour parameter.</typeparam>
    public interface IBehaviour<TParam>
    {
        /// <summary>
        /// Executes encapsulated behaviour.
        /// </summary>
        /// <param name="param">Parameter of execution.</param>
        void Execute(TParam param);
    }

    /// <summary>
    /// Represent encapsulated action (behaviour) which can be executed by 
    /// technology-independent moduels.
    /// </summary>
    /// <typeparam name="TParam">Type of behaviour parameter.</typeparam>
    /// <typeparam name="TResult">Type of value returned as a result of behaviour execution.</typeparam>
    public interface IBehaviour<TParam, TResult>
    {
        /// <summary>
        /// Executes encapsulated behaviour.
        /// </summary>
        /// <param name="param">Parameter of execution.</param>
        TResult Execute(TParam param);
    }
}
