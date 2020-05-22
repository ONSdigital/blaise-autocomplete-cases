using Blaise.Nuget.PubSub.Contracts.Interfaces;

namespace BlaiseAutoCompleteCases.Interfaces.Services
{
    public interface IQueueService
    {
        void CancelAllSubscriptions();
        void PublishMessage(string message);
        void Subscribe(IMessageHandler messageHandler);
    }
}