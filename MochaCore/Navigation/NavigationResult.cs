namespace MochaCore.Navigation
{
    /// <summary>
    /// Describes the possible outcomes of the navigation process.
    /// </summary>
    public enum NavigationResult
    {
        /// <summary>
        /// Navigation completed successfully.
        /// </summary>
        Succeed,

        /// <summary>
        /// Navigation process was rejected by currently active <see cref="INavigationModule"/>.
        /// </summary>
        RejectedByCurrent,

        /// <summary>
        /// Navigation process was rejected by requested <see cref="INavigationModule"/>.
        /// </summary>
        RejectedByTarget,

        /// <summary>
        /// Navigation process was aborted because currently active <see cref="INavigationModule"/> was requested.
        /// </summary>
        SameModuleRequested,

        /// <summary>
        /// Navigation process was aborted because new navigation request was made before current one could complete.
        /// </summary>
        RejectedByNewRequest
    }
}
