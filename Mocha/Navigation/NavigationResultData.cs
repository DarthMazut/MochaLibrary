using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mocha.Navigation
{
    /// <summary>
    /// Contains information about navigation result.
    /// </summary>
    public class NavigationResultData
    {
        /// <summary>
        /// Specifies the outcome of navigation process.
        /// </summary>
        public NavigationResult Result { get; }

        /// <summary>
        /// Describes the cause of navigation ending.
        /// </summary>
        public string Reason { get; } = string.Empty;

        /// <summary>
        /// Returns new instance of <see cref="NavigationResultData"/> class.
        /// </summary>
        /// <param name="result">Specifies the outcome of navigation process.</param>
        public NavigationResultData(NavigationResult result)
        {
            Result = result;
        }

        /// <summary>
        /// Returns new instance of <see cref="NavigationResultData"/> class.
        /// </summary>
        /// <param name="result">Specifies the outcome of navigation process.</param>
        /// <param name="reason">Describes the cause of navigation ending.</param>
        public NavigationResultData(NavigationResult result, string reason)
        {
            Result = result;
            Reason = reason;
        }
    }
}
