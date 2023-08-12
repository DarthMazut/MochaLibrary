namespace MochaCore.Windowing
{
    /// <summary>
    /// Marks implementing type as able to perform minimalization operation.
    /// </summary>
    public interface IMinimizeWindow
    {
        /// <summary>
        /// Minimizes related window.
        /// </summary>
        public void Minimize();
    }
}
