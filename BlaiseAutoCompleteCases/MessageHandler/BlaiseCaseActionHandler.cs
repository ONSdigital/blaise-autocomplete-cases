using Blaise.Nuget.PubSub.Contracts.Interfaces;
using BlaiseAutoCompleteCases.Interfaces.Mappers;
using BlaiseAutoCompleteCases.Interfaces.Services;
using log4net;
using System;
using BlaiseCaseReader.Interfaces.Mappers;

namespace BlaiseAutoCompleteCases.MessageHandler
{
    public class BlaiseCaseActionHandler : IMessageHandler
    {
        private readonly ILog _logger;
        private readonly IActionModelMapper _mapper;
        private readonly IProcessCompletionService _ProcessCompletionService;      

        public BlaiseCaseActionHandler(
            ILog logger,
            IActionModelMapper mapper,
            IProcessCompletionService ProcessCompletionService)
        {
            _logger = logger;
            _mapper = mapper;
            _ProcessCompletionService = ProcessCompletionService;
        }

        public bool HandleMessage(string message)
        {
            try
            {
                _logger.Info($"Message received - {message}");
                var actionModel = _mapper.MapToActionModel(message);                
                var actionStatus = _ProcessCompletionService.ProcessCompletion(actionModel);
                
                _logger.Info($"Action status - {actionStatus}");
                _logger.Info($"Message processed - {message}");

                return true;
            }
            catch (Exception e)
            {
                _logger.Error(e);

                return false;
            }                       
        }        
    }
}
