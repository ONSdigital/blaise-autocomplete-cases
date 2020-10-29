using Blaise.Case.Auto.Populate.Helpers;
using Blaise.Case.Auto.Populate.Interfaces.Services;
using Blaise.Case.Auto.Populate.Models;
using Blaise.Nuget.Api.Contracts.Interfaces;
using log4net;
using StatNeth.Blaise.API.DataLink;

namespace Blaise.Case.Auto.Populate.Services
{
    public class PopulateCasesService : IPopulateCasesService
    {
        private readonly ILog _logger;
        private readonly IFluentBlaiseApi _blaiseApi;
        private readonly IPopulateCaseService _populateCaseService;

        public PopulateCasesService(
            ILog logger,
            IFluentBlaiseApi blaiseApi,
            IPopulateCaseService populateCaseService)
        {
            _logger = logger;
            _blaiseApi = blaiseApi;
            _populateCaseService = populateCaseService;
        }

        public void PopulateCases(AutoPopulateCaseModel model)
        {
            model.InstrumentName.ThrowExceptionIfNullOrEmpty("InstrumentName");
            model.NumberOfCases.ThrowExceptionIfLessThanOrEqualToZero("NumberOfCases");

            var casePopulatedCounter = 0;

            if (!_blaiseApi
                .WithInstrument(model.InstrumentName)
                .WithServerPark(model.ServerPark)
                .WithConnection(_blaiseApi.DefaultConnection)
                .Survey
                .Exists)
            {
                _logger.Warn($"The survey '{model.InstrumentName}' does not exist on server park '{model.ServerPark}'");
                return;
            }

            _logger.Info($"Getting cases for survey '{model.InstrumentName}' on server park '{model.ServerPark}'");
            var dataSet = GetCases(model);

            while (!dataSet.EndOfSet)
            {
                if (model.NumberOfCases == casePopulatedCounter) break;

                _populateCaseService.PopulateCase(dataSet.ActiveRecord, model);

                casePopulatedCounter++;
                dataSet.MoveNext();
            }

            _logger.Info(casePopulatedCounter == 0
                ? "No Cases Found to populate"
                : $"Populated {casePopulatedCounter} cases");
        }

        private IDataSet GetCases(AutoPopulateCaseModel caseModel)
        {
            return _blaiseApi
                .WithConnection(_blaiseApi.DefaultConnection)
                .WithInstrument(caseModel.InstrumentName)
                .WithServerPark(caseModel.ServerPark)
                .Cases;
        }
    }
}