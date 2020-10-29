using Blaise.Nuget.PubSub.Contracts.Interfaces;

namespace Blaise.Case.Auto.Populate.Interfaces.Services
{
    public interface IQueueService
    {
        void CancelAllSubscriptions();
        void Subscribe(IMessageHandler messageHandler);
    }
}