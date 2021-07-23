namespace MochaCore.Navigation
{
    /// <summary>
    /// Represents an abstract, technology independent navigation unit.
    /// Encapsulates technology-specific view object and its data context.
    /// Provides methods and properties for working with contained objects.
    /// </summary>
    public interface INavigationModule
    {
        /// <summary>
        /// A technology-specific view object.
        /// </summary>
        object? View { get; }

        /// <summary>
        /// An <see cref="INavigatable"/> object bounded to <see cref="View"/>
        /// instance by DataBinding mechanism.
        /// </summary>
        INavigatable DataContext { get; }

        /// <summary>
        /// Sets a data context for technology-specific <see cref="View"/> object.
        /// </summary>
        /// <param name="dataContext">
        /// An object implementing <see cref="INavigatable"/> that will serve as data context
        /// for technology-specific <see cref="View"/> object.
        /// </param>
        void SetDataContext(INavigatable dataContext);

        /// <summary>
        /// Performs an action allowing this object to be removed
        /// by GC at later time. Clears technology-specific binding between 
        /// <see cref="View"/> object and its data context (<see cref="INavigatable"/>).
        /// This method will be called internally by <see cref="NavigationService"/>.
        /// </summary>
        void CleanUp();
    }
}
