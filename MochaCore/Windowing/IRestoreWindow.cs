namespace MochaCore.Windowing
{
    /// <summary>
    /// Marks implementing type as able to perform restore window operation.
    /// </summary>
    public interface IRestoreWindow
    {
        /// <summary>
        /// Resotres the window to default state and bring it to the top.
        /// </summary>
        public void Restore();
    }
}