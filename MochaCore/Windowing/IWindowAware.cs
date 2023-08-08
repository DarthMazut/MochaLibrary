namespace MochaCore.Windowing
{
    /// <summary>
    /// Marks implementing class as data context for <see cref="IWindowModule"/>.
    /// Allows for managing associated window.
    /// </summary>
    public interface IWindowAware
    {
        /// <summary>
        /// Provides API for managing associated window. 
        /// </summary>
        public IWindowControl WindowControl { get; }
    }
}