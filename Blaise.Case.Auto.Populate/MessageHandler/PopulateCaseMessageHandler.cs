using System;
using Blaise.Case.Auto.Populate.Interfaces.Mappers;
using Blaise.Case.Auto.Populate.Interfaces.Services;
using Blaise.Nuget.PubSub.Contracts.Interfaces;
using log4net;

namespace Blaise.Case.Auto.Populate.MessageHandler
{
    public class PopulateCaseMessageHandler : IMessageHandler
    {
        private readonly ILog _logger;
        private readonly IModelMapper _mapper;
        private readonly IPopulateCasesService _populateCasesService;
        private readonly IPopulateCaseService _populateCaseService;

        public PopulateCaseMessageHandler(
            ILog logger,
            IModelMapper mapper, 
            IPopulateCasesService populateCasesService, 
            IPopulateCaseService populateCaseService)
        {
            _logger = logger;
            _mapper = mapper;
            _populateCasesService = populateCasesService;
            _populateCaseService = populateCaseService;
        }

        public bool HandleMessage(string message)
        {
            try
            {
                _logger.Info($"Message received '{message}'");

                var model = _mapper.MapToModel(message);

                if (model.IsSpecificCase)
                {
                    _populateCaseService.PopulateCase(model);
                }
                else
                {
                    _populateCasesService.PopulateCases(model);
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
