using System;
using System.Collections.Generic;
using System.Linq;
using Blaise.Nuget.Api.Contracts.Interfaces;
using BlaiseAutoCompleteCases.Helpers;
using BlaiseAutoCompleteCases.Interfaces.Services;
using log4net;
using StatNeth.Blaise.API.DataRecord;
using StatNeth.Blaise.API.ServerManager;

namespace BlaiseAutoCompleteCases.Services
{
    public class FindCasesToCompleteService : IFindCasesToCompleteService
    {
        private readonly ILog _logger;
        private readonly IBlaiseApi _blaiseApi;
        private readonly ICompleteCaseService _completeCaseService;

        public FindCasesToCompleteService(
            ILog logger, 
            IBlaiseApi blaiseApi, 
            ICompleteCaseService completeCaseService)
        {
            _logger = logger;
            _blaiseApi = blaiseApi;
            _completeCaseService = completeCaseService;
        }

        public void FindCasesToComplete(string surveyName, int numberOfCasesToComplete)
        {
            surveyName.ThrowExceptionIfNullOrEmpty("surveyName");
            numberOfCasesToComplete.ThrowExceptionIfLessThanOrEqualToZero("numberOfCasesToComplete");
            int caseCompletedCounter = 0;
            var surveys = _blaiseApi.GetAllSurveys().ToList();

            foreach (var survey in surveys)
            {
                if (numberOfCasesToComplete == caseCompletedCounter) continue;

                var dataSet = _blaiseApi.GetDataSet(survey.Name, survey.ServerPark);

                while (!dataSet.EndOfSet)
                {
                    if (!_blaiseApi.CaseHasBeenCompleted(dataSet.ActiveRecord))
                    {
                        _blaiseApi.MarkCaseAsComplete(dataSet.ActiveRecord, survey.Name, survey.ServerPark);
                        caseCompletedCounter++;
                    }
                    dataSet.MoveNext();
                }
            }

            if (caseCompletedCounter == 0)
            {
                _logger.Info($"No Cases Found to Complete");
            }
        }
    }
}