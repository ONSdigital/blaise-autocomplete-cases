using Blaise.Nuget.Api.Contracts.Interfaces;
using BlaiseAutoCompleteCases.Enums;
using BlaiseAutoCompleteCases.Interfaces.Mappers;
using BlaiseAutoCompleteCases.Interfaces.Services;
using BlaiseAutoCompleteCases.Models;
using log4net;

namespace BlaiseAutoCompleteCases.Services
{
    public class ProcessCompletionService : IProcessCompletionService
    {
        private readonly ILog _logger;
        private readonly IBlaiseApi _blaiseApi;
        private readonly IPublishValidationRequestService _publishService;
        private readonly IValidationModelMapper _mapper;

        public ProcessCompletionService(
            ILog logger,
            IBlaiseApi blaiseApi,
            IPublishValidationRequestService publishService,
            IValidationModelMapper mapper)
        {
            _logger = logger;
            _blaiseApi = blaiseApi;
            _publishService = publishService;
            _mapper = mapper;
        }

        public CompletionStatusType ProcessCompletion(ActionModel actionmodel)
        {


            foreach (var survey in _blaiseApi.GetAllSurveys())
            {

            }

            return CompletionStatusType.CaseAutoCompleted;
        }
    }
}



