using Blaise.Nuget.Api.Contracts.Interfaces;
using BlaiseAutoCompleteCases.Interfaces.Services;
using log4net;

namespace BlaiseAutoCompleteCases.Services
{
    public class FindCasesToCompleteService : IFindCasesToCompleteService
    {
        private readonly ILog _logger;
        private readonly IBlaiseApiNew _blaiseApi;
        private readonly ICompleteCaseService _completeCaseService;

        public FindCasesToCompleteService(
            ILog logger, 
            IBlaiseApiNew blaiseApi, 
            ICompleteCaseService completeCaseService)
        {
            _logger = logger;
            _blaiseApi = blaiseApi;
            _completeCaseService = completeCaseService;
        }

        public void FindCasesToComplete(string surveyName, int numberOfCasesToComplete)
        {
            foreach (var survey in _blaiseApi.GetAllSurveys())
            {
                if (!_blaiseApi.IsCaseComplete(survey.Name, survey.ServerPark))
                {
                    //Call service to complete case etc..
                }
            }
        }
    }
}