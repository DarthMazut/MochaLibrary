namespace MochaCore.Windowing
{
    /// <summary>
    /// Marks implementing type as able to expose window state.
    /// </summary>
    public interface IWindowStateAware
    {
        /// <summary>
        /// Returns current window state.
        /// </summary>
        public ModuleWindowState WindowState { get; }
    }
}