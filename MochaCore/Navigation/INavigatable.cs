namespace MochaCore.Navigation
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
