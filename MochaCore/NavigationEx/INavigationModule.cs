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
        /// Provides a unified identifier for <see cref="INavigationModule"/> implementation which represents
        /// application root. 
        /// </summary>
        public static readonly string RootId = "/";

        /// <summary>
        /// Occurs when an <see cref="INavigationModule"/> is initialized.
        /// </summary>
        public event EventHandler? Initialized;

        /// <summary>
        /// Occurs when an <see cref="INavigationModule"/> is uninitialized.
        /// </summary>
        public event EventHandler? Uninitialized;

        /// <summary>
        /// Unique identifier of this module.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Determines whether this <see cref="INavigationModule"/> is initialized.
        /// Initialized module should contain non-null values of <see cref="View"/>
        /// and <see cref="DataContext"/> properties.
        /// </summary>
        public bool IsInitialized { get; }

        /// <summary>
        /// A technology-specific view object of encapsulated navigation unit.
        /// </summary>
        public object? View { get; }

        /// <summary>
        /// Returns type of <see cref="View"/> associated with this <see cref="INavigationModule"/>.
        /// </summary>
        public Type ViewType { get; }

        /// <summary>
        /// An <see cref="INavigatable"/> object bounded to <see cref="View"/>
        /// instance by <i>DataBinding</i> mechanism.
        /// </summary>
        public INavigatable? DataContext { get; }

        /// <summary>
        /// Returns type of <see cref="DataContext"/> associated with this <see cref="INavigationModule"/>.
        /// </summary>
        public Type DataContextType { get; }
    }
}
