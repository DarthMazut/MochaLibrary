using System;

namespace MochaCore.Windowing
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICustomWindowAware : IWindowAware
    {
        IWindowControl IWindowAware.WindowControl => WindowControl;

        /// <summary>
        /// Provides API form managing related window.
        /// </summary>
        public new ICustomWindowControl WindowControl { get; }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICustomWindowAware<T> : ICustomWindowAware, IWindowAware<T> where T : class, new()
    {
        ICustomWindowControl ICustomWindowAware.WindowControl => WindowControl;

        IWindowControl<T> IWindowAware<T>.WindowControl => WindowControl;

        /// <summary>
        /// Provides API form managing related window.
        /// </summary>
        public new ICustomWindowControl<T> WindowControl { get; }
    }
}