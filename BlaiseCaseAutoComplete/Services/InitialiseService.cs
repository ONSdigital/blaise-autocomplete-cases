using System;
using System.Linq;
using Blaise.Nuget.Api.Contracts.Interfaces;
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
        private readonly IBlaiseApi _blaiseApi;

        public InitialiseService(
            ILog logger,
            IQueueService queueService,
            IMessageHandler messageHandler, 
            IBlaiseApi blaiseApi)
        {
            _logger = logger;
            _queueService = queueService;
            _messageHandler = messageHandler;
            _blaiseApi = blaiseApi;
        }

        public void Start()
        {
            try
            {
                LogAllServerParksOnVm();
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

        private void LogAllServerParksOnVm()
        {
            var serverParkNames = _blaiseApi.GetServerParkNames().ToList();

            _logger.Info($"Found '{serverParkNames.Count}' server parks");

            foreach (var serverParkName in serverParkNames)
            {
                _logger.Info($"{serverParkName}");
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
