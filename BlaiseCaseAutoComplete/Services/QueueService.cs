using Blaise.Nuget.PubSub.Contracts.Interfaces;
using BlaiseCaseAutoComplete.Interfaces.Providers;
using BlaiseCaseAutoComplete.Interfaces.Services;

namespace BlaiseCaseAutoComplete.Services
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
            var subscriptionId = $"{_configurationProvider.SubscriptionId}-{_configurationProvider.VmName}";

            _queueProvider
                .WithProject(_configurationProvider.ProjectId)
                .WithTopic(_configurationProvider.SubscriptionTopicId)
                .CreateSubscription(subscriptionId)
                .StartConsuming(messageHandler);
        }

        public void CancelAllSubscriptions()
        {
            _queueProvider
                .StopConsuming();
        }
    }
}
