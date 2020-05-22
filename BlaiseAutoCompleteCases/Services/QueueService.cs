using Blaise.Nuget.PubSub.Contracts.Interfaces;
using BlaiseAutoCompleteCases.Interfaces.Providers;
using BlaiseAutoCompleteCases.Interfaces.Services;

namespace BlaiseAutoCompleteCases.Services
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
                .ForProject(_configurationProvider.ProjectId)
                .ForSubscription(_configurationProvider.SubscriptionId)
                .StartConsuming(messageHandler);
        }

        public void PublishMessage(string message)
        {
            _queueProvider
                .ForProject(_configurationProvider.ProjectId)
                .ForTopic(_configurationProvider.TopicId)
                .Publish(message);
        }

        public void CancelAllSubscriptions()
        {
            _queueProvider
                .StopConsuming();
        }
    }
}
