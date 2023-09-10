namespace MochaCore.Windowing
{
    /// <summary>
    /// Marks implementing type as able to perform hide operation.
    /// </summary>
    public interface IHideWindow
    {
        /// <summary>
        /// Hides the window so it's no longer visible to the user.
        /// Hiding the window is differen operation than closing it.
        /// Hidden window can be displayed by calling <see cref="IRestoreWindow.Restore"/> method.
        /// </summary>
        public void Hide();
    }
}