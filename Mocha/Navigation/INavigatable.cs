using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mocha.Navigation
{
    /// <summary>
    /// Allows implementing class to participate in the navigation process. 
    /// </summary>
    public interface INavigatable
    {
        /// <summary>
        /// Exposes API for navigation.
        /// </summary>
        Navigator Navigator { get; }
    }
}
