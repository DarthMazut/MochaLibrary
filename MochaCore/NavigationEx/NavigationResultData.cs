using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.NavigationEx
{
    /// <summary>
    /// Contains a data describing navigation process outcome.
    /// </summary>
    public class NavigationResultData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationRequestData"/> class.
        /// </summary>
        /// <param name="result">Describes the possible outcomes of the navigation process.</param>
        public NavigationResultData(NavigationResult result) : this(result, null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationRequestData"/> class.
        /// </summary>
        /// <param name="result">Describes the possible outcomes of the navigation process.</param>
        /// <param name="data">Additional data.</param>
        public NavigationResultData(NavigationResult result, object? data)
        {
            Data = data;
            Result = result;
        }

        /// <summary>
        /// Describes the possible outcomes of the navigation process.
        /// </summary>
        public NavigationResult Result { get; }

        /// <summary>
        /// Additional data returned from the navigation process.
        /// For <see cref="NavigationType.Pop"/> requests, this property may contain a value.
        /// </summary>
        public object? Data { get; }
    }
}
