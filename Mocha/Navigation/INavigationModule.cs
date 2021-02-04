using System;
using System.Collections.Generic;
using System.Text;

namespace Mocha.Navigation
{
    /// <summary>
    /// Represents an abstract, technology independent view object.
    /// Provides methods and properties for working with represented object.
    /// </summary>
    public interface INavigationModule
    {
        /// <summary>
        /// A technology-specific view object.
        /// </summary>
        object View { get; }

        /// <summary>
        /// An <see cref="INavigatable"/> object bounded to <see cref="View"/>
        /// instance by *DataBinding* mechanism.
        /// </summary>
        INavigatable DataContext { get; }

        /// <summary>
        /// Allows to set a *DataContext* for technology-specific <see cref="View"/> object.
        /// </summary>
        /// <param name="dataContext">A *DataContext* object which will be bind to 
        /// <see cref="View"/> by *DataBinding* mechanism</param>
        void SetDataContext(INavigatable dataContext);

        /// <summary>
        /// Performs and action allowing this object to be removed
        /// by GC at later time. It is important to set <see cref="DataContext"/>
        /// to null. This method will be called internally by <see cref="NavigationService"/>.
        /// </summary>
        void CleanUp();
    }
}
