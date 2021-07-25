namespace MochaCore.Events.Extensions.DI
{
    /// <inheritdoc/>
    public class EventService : IEventService
    {
        /// <inheritdoc/>
        public IEventProvider<TEventArgs> RequestEventProvider<TEventArgs>(string id) where TEventArgs : BaseEventArgs
        {
            return AppEventManager.RequestEventProvider<TEventArgs>(id);
        }
    }
}
