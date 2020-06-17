using System.Linq;
using Blaise.Nuget.Api.Contracts.Interfaces;
using BlaiseCaseAutoComplete.Helpers;
using BlaiseCaseAutoComplete.Interfaces.Services;
using log4net;

namespace BlaiseCaseAutoComplete.Services
{
    public class CompleteCasesService : ICompleteCasesService
    {
        private readonly ILog _logger;
        private readonly IBlaiseApi _blaiseApi;
        private readonly ICompleteCaseService _completeCaseService;

        public CompleteCasesService(
            ILog logger, 
            IBlaiseApi blaiseApi, 
            ICompleteCaseService completeCaseService)
        {
            _logger = logger;
            _blaiseApi = blaiseApi;
            _completeCaseService = completeCaseService;
        }

        public void CompleteCases(string surveyName, int numberOfCasesToComplete)
        {
            surveyName.ThrowExceptionIfNullOrEmpty("surveyName");
            numberOfCasesToComplete.ThrowExceptionIfLessThanOrEqualToZero("numberOfCasesToComplete");
            var caseCompletedCounter = 0;
            var surveys = _blaiseApi.GetAllSurveys().ToList();

            _logger.Info($"Found '{surveys.Count}' surveys");

            foreach (var survey in surveys)
            {
                _logger.Info($"Getting cases for survey '{survey.Name}' on server park '{survey.ServerPark}'");
                var dataSet = _blaiseApi.GetDataSet(survey.Name, survey.ServerPark);

                while (!dataSet.EndOfSet)
                {
                    if (numberOfCasesToComplete == caseCompletedCounter) break;

                    if (!_blaiseApi.CaseHasBeenCompleted(dataSet.ActiveRecord))
                    {
                        _completeCaseService.CompleteCase(dataSet.ActiveRecord, survey.Name, survey.ServerPark);
                        caseCompletedCounter++;
                    }
                    dataSet.MoveNext();
                }
            }

            if (caseCompletedCounter == 0)
            {
                _logger.Info("No Cases Found to Complete");
            }
        }
    }
}