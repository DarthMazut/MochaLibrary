using Mocha.Events.Extensions;
using System;
using System.Threading.Tasks;

namespace Mocha.Events
{
    /// <summary>
    /// Shares technology specific events which can be subsribed to.
    /// </summary>
    public interface IEventProvider
    {
        /// <summary>
        /// Fires whenever application is about to close.
        /// </summary>
        event EventHandler<AppClosingEventArgs> AppClosing;

        /// <summary>
        /// Fires after app was closed.
        /// </summary>
        event EventHandler AppClosed;

        /// <summary>
        /// Fires whenever user presses mobile device *Back Button*.
        /// </summary>
        event EventHandler<HardwareBackButtonPressedEventArgs> HardwareBackButtonPressed;

        /// <summary>
        /// Stores <see cref="Task"/> which will be executed whenever application is about to close.
        /// </summary>
        /// <param name="task"></param>
        void SubscribeAsyncAppClosing(Func<Task<AppClosingEventArgs>> task);
    }
}