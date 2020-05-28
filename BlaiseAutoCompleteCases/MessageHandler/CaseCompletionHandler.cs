using System;
using Blaise.Nuget.PubSub.Contracts.Interfaces;
using BlaiseAutoCompleteCases.Interfaces.Mappers;
using BlaiseAutoCompleteCases.Interfaces.Services;
using log4net;

namespace BlaiseAutoCompleteCases.MessageHandler
{
    public class CaseCompletionHandler : IMessageHandler
    {
        private readonly ILog _logger;
        private readonly ICompletionModelMapper _completionModelMapper;
        private readonly ICompleteCasesService _findCasesToCompleteService;

        public CaseCompletionHandler(
            ILog logger,
            ICompletionModelMapper completionModelMapper, 
            ICompleteCasesService findCasesToCompleteService)
        {
            _logger = logger;
            _completionModelMapper = completionModelMapper;
            _findCasesToCompleteService = findCasesToCompleteService;
        }

        public bool HandleMessage(string message)
        {
            try
            {
                _logger.Info($"Message received '{message}'");

                //Calling mapper here
                var completionModel = _completionModelMapper.MapToCompletionModel(message);

                //calling find cases to complete service
                _findCasesToCompleteService.CompleteCases(completionModel.SurveyName, completionModel.NumberOfCasesToComplete);

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
