using Blaise.Nuget.PubSub.Contracts.Interfaces;

namespace BlaiseCaseAutoComplete.Interfaces.Services
{
    public interface IQueueService
    {
        void CancelAllSubscriptions();
        void Subscribe(IMessageHandler messageHandler);
    }
}