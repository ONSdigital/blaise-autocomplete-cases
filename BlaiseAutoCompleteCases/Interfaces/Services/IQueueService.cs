using Blaise.Nuget.PubSub.Contracts.Interfaces;

namespace BlaiseAutoCompleteCases.Interfaces.Services
{
    public interface IQueueService
    {
        void CancelAllSubscriptions();
        void Subscribe(IMessageHandler messageHandler);
    }
}