using Blaise.Nuget.Api.Contracts.Interfaces;
using BlaiseAutoCompleteCases.Helpers;
using BlaiseAutoCompleteCases.Interfaces.Services;
using log4net;
using System.Linq;

namespace BlaiseAutoCompleteCases.Services
{
    public class CompleteCasesService : ICompleteCasesService
    {
        private readonly ILog _logger;
        private readonly IBlaiseApi _blaiseApi;
        private readonly ICompleteCaseService _completeCaseService;
        private readonly IDataService _iDataService;

        public CompleteCasesService(
            ILog logger, 
            IBlaiseApi blaiseApi, 
            ICompleteCaseService completeCaseService, IDataService iDataService)
        {
            _logger = logger;
            _blaiseApi = blaiseApi;
            _completeCaseService = completeCaseService;
            _iDataService = iDataService;
        }

        public void CompleteCases(string surveyName, int numberOfCasesToComplete)
        {
            surveyName.ThrowExceptionIfNullOrEmpty("surveyName");
            numberOfCasesToComplete.ThrowExceptionIfLessThanOrEqualToZero("numberOfCasesToComplete");
            int caseCompletedCounter = 0;
            var surveys = _blaiseApi.GetAllSurveys().ToList();

            foreach (var survey in surveys)
            {
                var dataSet = _blaiseApi.GetDataSet(survey.Name, survey.ServerPark);

                while (!dataSet.EndOfSet)
                {
                    if (numberOfCasesToComplete == caseCompletedCounter) break;

                    var dataRecord = dataSet.ActiveRecord;
                    if (!_blaiseApi.CaseHasBeenCompleted(dataSet.ActiveRecord))
                    {
                        _iDataService.GetDataAndUpdate(dataRecord, survey.Name, survey.ServerPark);
                        _completeCaseService.CompleteCase(dataRecord, survey.Name, survey.ServerPark);
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