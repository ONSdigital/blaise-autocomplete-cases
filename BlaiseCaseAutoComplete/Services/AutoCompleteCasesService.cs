using System;
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

            try
            {
                if (!_blaiseApi
                    .WithInstrument(model.InstrumentName)
                    .WithServerPark(model.ServerPark)
                    .WithConnection(_blaiseApi.DefaultConnection)
                    .Survey
                    .Exists)
                {
                    _logger.Warn(
                        $"The survey '{model.InstrumentName}' does not exist on server park '{model.ServerPark}'");
                    return;
                }

                _logger.Info($"Getting cases for survey '{model.InstrumentName}' on server park '{model.ServerPark}'");
                var dataSet = GetCases(model);

                while (!dataSet.EndOfSet)
                {
                    if (model.NumberOfCases == caseCompletedCounter) break;

                    if (!CaseIsComplete(dataSet.ActiveRecord))
                    {
                        _completeCaseService.CompleteCase(dataSet.ActiveRecord, model);

                        caseCompletedCounter++;
                    }

                    dataSet.MoveNext();
                }
            }
            catch (Exception e)
            {
                _logger.Info(e.Message, e.InnerException);
                throw;
            }
            finally
            {
                _logger.Info(caseCompletedCounter == 0
                    ? "No Cases Found to Complete"
                    : $"Completed {caseCompletedCounter} cases");
            }
        }

        private IDataSet GetCases(AutoCompleteCaseModel caseModel)
        {
            return _blaiseApi
                .WithConnection(_blaiseApi.DefaultConnection)
                .WithInstrument(caseModel.InstrumentName)
                .WithServerPark(caseModel.ServerPark)
                .Cases;
        }

        private bool CaseIsComplete(IDataRecord dataRecord)
        {
            return _blaiseApi
                .Case
                .WithDataRecord(dataRecord)
                .Completed;
        }
    }
}