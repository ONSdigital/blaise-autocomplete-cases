using System;
using System.Linq;
using Blaise.Case.Auto.Populate.Interfaces.Providers;
using Blaise.Case.Auto.Populate.Interfaces.Services;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Blaise.Nuget.PubSub.Contracts.Interfaces;
using log4net;

namespace Blaise.Case.Auto.Populate.Services
{
    public class InitialiseService : IInitialiseService
    {
        private readonly ILog _logger;
        private readonly IQueueService _queueService;
        private readonly IMessageHandler _messageHandler;
        private readonly IFluentBlaiseApi _blaiseApi;
        private readonly IConfigurationProvider _configurationProvider;

        public InitialiseService(
            ILog logger,
            IQueueService queueService,
            IMessageHandler messageHandler,
            IFluentBlaiseApi blaiseApi, 
            IConfigurationProvider configurationProvider)
        {
            _logger = logger;
            _queueService = queueService;
            _messageHandler = messageHandler;
            _blaiseApi = blaiseApi;
            _configurationProvider = configurationProvider;
        }

        public void Start()
        {
            try
            {
                LogAllServerParksOnVm();
                _logger.Info($"Subscribing to '{_configurationProvider.SubscriptionId}' on project'{_configurationProvider.ProjectId}'");

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
            var serverParkNames = _blaiseApi
                .WithConnection(_blaiseApi.DefaultConnection)
                .ServerParks.ToList();

            _logger.Info($"Found '{serverParkNames.Count}' server parks");

            foreach (var serverParkName in serverParkNames)
            {
                _logger.Info($"{serverParkName}");
            }
        }

        public void Stop()
        {
            _logger.Info($"Stopping subscription to '{_configurationProvider.SubscriptionId}'");
            _queueService.CancelAllSubscriptions();
            _logger.Info("Subscription stopped");
        }
    }
}
