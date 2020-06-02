﻿using Blaise.Nuget.PubSub.Contracts.Interfaces;
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
            _queueProvider
                .ForProject(_configurationProvider.ProjectId)
                .ForSubscription(_configurationProvider.SubscriptionId)
                .StartConsuming(messageHandler);
        }

        public void CancelAllSubscriptions()
        {
            _queueProvider
                .StopConsuming();
        }
    }
}