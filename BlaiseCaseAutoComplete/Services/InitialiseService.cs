using System;
using Blaise.Nuget.PubSub.Contracts.Interfaces;
using BlaiseCaseAutoComplete.Interfaces.Services;
using log4net;

namespace BlaiseCaseAutoComplete.Services
{
    public class InitialiseService : IInitialiseService
    {
        private readonly ILog _logger;
        private readonly IQueueService _queueService;
        private readonly IMessageHandler _messageHandler;

        public InitialiseService(
            ILog logger,
            IQueueService queueService,
            IMessageHandler messageHandler)
        {
            _logger = logger;
            _queueService = queueService;
            _messageHandler = messageHandler;
        }

        public void Start()
        {
            try
            {
                _logger.Info("Subscribing to topic");
                _queueService.Subscribe(_messageHandler);

                //System.Threading.Thread.Sleep(1200000);

                _logger.Info("Subscription setup");
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        public void Stop()
        {
            _logger.Info("Stopping subscription to topic - updated");
            _queueService.CancelAllSubscriptions();
            _logger.Info("Subscription stopped - updated");
        }
    }
}
