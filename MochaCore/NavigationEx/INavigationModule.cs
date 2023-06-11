using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.NavigationEx
{
    /// <summary>
    /// Represents an abstract, technology independent navigation unit.
    /// Encapsulates technology-specific view object and its data context.
    /// </summary>
    public interface INavigationModule
    {
        /// <summary>
        /// Unique identifier of this module.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Determines whether this <see cref="INavigationModule"/> is initialized.
        /// Initialized module contains non-null value of at least <see cref="View"/>
        /// property.
        /// </summary>
        public bool IsInitialized { get; }

        /// <summary>
        /// A technology-specific view object.
        /// </summary>
        public object? View { get; }

        /// <summary>
        /// An <see cref="INavigatable"/> object bounded to <see cref="View"/>
        /// instance by <i>DataBinding</i> mechanism.
        /// </summary>
        public INavigatable? DataContext { get; }
    }
}
