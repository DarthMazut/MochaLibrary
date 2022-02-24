namespace MochaCore.Navigation
{
    /// <summary>
    /// Implemented by <see cref="Navigator"/> class. Provides a getter 
    /// for currently active technology-specific view object associated with 
    /// host of <see cref="Navigator"/> instance implementing this interface.
    /// </summary>
    public interface INavigatorGetHostView
    {
        /// <summary>
        /// Returns technology-specific counterpart of host for <see cref="Navigator"/>
        /// which exposes this property.
        /// </summary>
        object? HostView { get; }
    }
}