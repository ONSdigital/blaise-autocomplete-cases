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
        private readonly IFindCasesToCompleteService _findCasesToCompleteService;

        public CaseCompletionHandler(
            ILog logger,
            ICompletionModelMapper completionModelMapper, 
            IFindCasesToCompleteService findCasesToCompleteService)
        {
            _logger = logger;
            _completionModelMapper = completionModelMapper;
            _findCasesToCompleteService = findCasesToCompleteService;
        }

        public bool HandleMessage(string message)
        {
            try
            {

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
