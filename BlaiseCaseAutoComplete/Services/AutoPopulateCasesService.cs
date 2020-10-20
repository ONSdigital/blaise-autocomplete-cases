using System;
using Blaise.Nuget.Api.Contracts.Interfaces;
using BlaiseCaseAutoComplete.Helpers;
using BlaiseCaseAutoComplete.Interfaces.Services;
using BlaiseCaseAutoComplete.Models;
using log4net;
using StatNeth.Blaise.API.DataLink;

namespace BlaiseCaseAutoComplete.Services
{
    public class AutoPopulateCasesService : IAutoPopulateCasesService
    {
        private readonly ILog _logger;
        private readonly IFluentBlaiseApi _blaiseApi;
        private readonly IPopulateCaseService _populateCaseService;

        public AutoPopulateCasesService(
            ILog logger,
            IFluentBlaiseApi blaiseApi,
            IPopulateCaseService populateCaseService)
        {
            _logger = logger;
            _blaiseApi = blaiseApi;
            _populateCaseService = populateCaseService;
        }

        public void PopulateCases(AutoCompleteCaseModel model)
        {
            model.InstrumentName.ThrowExceptionIfNullOrEmpty("InstrumentName");
            model.NumberOfCases.ThrowExceptionIfLessThanOrEqualToZero("NumberOfCases");

            var caseCompletedCounter = 0;
            var primaryKey = "";
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

                    primaryKey = _blaiseApi.Case.WithDataRecord(dataSet.ActiveRecord).PrimaryKey;

                    _populateCaseService.CompleteCase(dataSet.ActiveRecord, model);

                    caseCompletedCounter++;

                    dataSet.MoveNext();
                }
            }
            catch (Exception e)
            {
                _logger.Error($"Error whilst processing primary key {primaryKey}");
                _logger.Error(e.Message, e.InnerException);
                throw;
            }
            finally
            {
                _logger.Info(caseCompletedCounter == 0
                    ? "No Cases Found to populate"
                    : $"Populated {caseCompletedCounter} cases");
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
    }
}