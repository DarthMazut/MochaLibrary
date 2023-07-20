namespace MochaCore.NavigationEx
{
    /// <summary>
    /// Allows implementing class to participate in the navigation process.
    /// Only <see cref="INavigationParticipant"/> implementations can serve as data 
    /// context for <see cref="INavigationModule"/>.
    /// </summary>
    public interface INavigationParticipant
    {
        /// <summary>
        /// Exposes API for navigation.
        /// </summary>
        public INavigator Navigator { get; }
    }
}