using System.Linq;
using Blaise.Nuget.Api.Contracts.Interfaces;
using BlaiseCaseAutoComplete.Helpers;
using BlaiseCaseAutoComplete.Interfaces.Services;
using BlaiseCaseAutoComplete.Models;
using log4net;
using StatNeth.Blaise.API.DataLink;
using StatNeth.Blaise.API.DataRecord;
using StatNeth.Blaise.API.ServerManager;

namespace BlaiseCaseAutoComplete.Services
{
    public class AutoCompleteCasesService : IAutoCompleteCasesService
    {
        private readonly ILog _logger;
        private readonly IFluentBlaiseApi _blaiseApi;
        private readonly ICompleteCaseService _completeCaseService;

        public AutoCompleteCasesService(
            ILog logger,
            IFluentBlaiseApi blaiseApi, 
            ICompleteCaseService completeCaseService)
        {
            _logger = logger;
            _blaiseApi = blaiseApi;
            _completeCaseService = completeCaseService;
        }

        public void CompleteCases(AutoCompleteCaseModel model)
        {
            model.InstrumentName.ThrowExceptionIfNullOrEmpty("InstrumentName");
            model.NumberOfCases.ThrowExceptionIfLessThanOrEqualToZero("NumberOfCases");

            var caseCompletedCounter = 0;
            var surveys = _blaiseApi.Surveys.ToList();

            _logger.Info($"Found '{surveys.Count}' surveys");

            foreach (var survey in surveys)
            {
                _logger.Info($"Getting cases for survey '{survey.Name}' on server park '{survey.ServerPark}'");
                var dataSet = GetCases(survey);

                while (!dataSet.EndOfSet)
                {
                    if (model.NumberOfCases == caseCompletedCounter) break;

                    if (!CaseIsComplete(dataSet.ActiveRecord, survey))
                    {
                        _completeCaseService.CompleteCase(dataSet.ActiveRecord, model);
                    
                        caseCompletedCounter++;
                    }
                    dataSet.MoveNext();
                }
            }

            if (caseCompletedCounter == 0)
            {
                _logger.Info("No Cases Found to Complete");
            }
            else
            {
                _logger.Info($"Completed {caseCompletedCounter} cases");
            }
        }

        private IDataSet GetCases(ISurvey survey)
        {
            return _blaiseApi
                .WithInstrument(survey.Name)
                .WithServerPark(survey.ServerPark)
                .Cases;
        }

        private bool CaseIsComplete(IDataRecord dataRecord, ISurvey survey)
        {
            return _blaiseApi
                .WithInstrument(survey.Name)
                .WithServerPark(survey.ServerPark)
                .Case
                .WithDataRecord(dataRecord)
                .Completed;
        }
    }
}