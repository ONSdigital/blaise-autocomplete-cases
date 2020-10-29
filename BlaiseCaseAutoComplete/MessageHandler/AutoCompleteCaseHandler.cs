using System;
using Blaise.Nuget.PubSub.Contracts.Interfaces;
using BlaiseCaseAutoComplete.Interfaces.Mappers;
using BlaiseCaseAutoComplete.Interfaces.Services;
using log4net;

namespace BlaiseCaseAutoComplete.MessageHandler
{
    public class AutoCompleteCaseHandler : IMessageHandler
    {
        private readonly ILog _logger;
        private readonly IModelMapper _mapper;
        private readonly IAutoCompleteCasesService _autoCompleteCasesService;
        private readonly ICompleteCaseService _completeCaseService;
        private readonly IQueueService _queueService;

        public AutoCompleteCaseHandler(
            ILog logger,
            IModelMapper mapper, 
            IAutoCompleteCasesService autoCompleteCasesService, 
            ICompleteCaseService completeCaseService, 
            IQueueService queueService)
        {
            _logger = logger;
            _mapper = mapper;
            _autoCompleteCasesService = autoCompleteCasesService;
            _completeCaseService = completeCaseService;
            _queueService = queueService;
        }

        public bool HandleMessage(string message)
        {
            try
            {
                _logger.Info($"Message received '{message}'");

                var model = _mapper.MapToModel(message);

                if (model.IsSpecificCase)
                {
                    _completeCaseService.CompleteCase(model);
                }
                else
                {
                    _autoCompleteCasesService.CompleteCases(model);
                }

                _logger.Info($"Message processed '{message}'");

                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                _logger.Info($"Error processing message '{message}'");

                return false;
            }                       
        }
    }
}
