using Blaise.Case.Auto.Populate.Interfaces.Providers;
using Blaise.Case.Auto.Populate.Interfaces.Services;
using Blaise.Nuget.PubSub.Contracts.Interfaces;

namespace Blaise.Case.Auto.Populate.Services
{
    public class QueueService : IQueueService
    {
        private readonly IConfigurationProvider _configurationProvider;
        private readonly IFluentQueueApi _queueProvider;

        public QueueService(
            IConfigurationProvider configurationProvider,
            IFluentQueueApi queueProvider)
        {
            _configurationProvider = configurationProvider;
            _queueProvider = queueProvider;
        }

        public void Subscribe(IMessageHandler messageHandler)
        {
            _queueProvider
                .WithProject(_configurationProvider.ProjectId)
                .WithSubscription(_configurationProvider.SubscriptionId)
                .StartConsuming(messageHandler);
        }

        public void CancelAllSubscriptions()
        {
            _queueProvider
                .StopConsuming();
        }
    }
}
