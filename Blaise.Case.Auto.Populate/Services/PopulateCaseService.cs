using Blaise.Case.Auto.Populate.Helpers;
using Blaise.Case.Auto.Populate.Interfaces.Services;
using Blaise.Case.Auto.Populate.Models;
using Blaise.Nuget.Api.Contracts.Interfaces;
using log4net;
using StatNeth.Blaise.API.DataRecord;

namespace Blaise.Case.Auto.Populate.Services
{
    public class PopulateCaseService : IPopulateCaseService
    {
        private readonly ILog _logger;
        private readonly IFluentBlaiseApi _blaiseApi;

        public PopulateCaseService(
            ILog logger,
            IFluentBlaiseApi blaiseApi)
        {
            _logger = logger;
            _blaiseApi = blaiseApi;
        }

        public void PopulateCase(AutoPopulateCaseModel model)
        {
            model.PrimaryKey.ThrowExceptionIfNullOrEmpty("PrimaryKey");
            model.InstrumentName.ThrowExceptionIfNullOrEmpty("InstrumentName");
            model.ServerPark.ThrowExceptionIfNullOrEmpty("ServerPark");

            if (!CaseExists(model))
            {
                _logger.Info(
                    $"Case with primary key '{model.PrimaryKey}' was not found for survey '{model.InstrumentName}' on ServeParkName: {model.ServerPark}");

                return;
            }

            var dataRecord = GetCase(model);
            PopulateCase(dataRecord, model);

            _logger.Info($"Populated case with primary key '{model.PrimaryKey}'");
        }

        public void PopulateCase(IDataRecord dataRecord, AutoPopulateCaseModel model)
        {
            model.InstrumentName.ThrowExceptionIfNullOrEmpty("InstrumentName");
            model.ServerPark.ThrowExceptionIfNullOrEmpty("ServerPark");

            UpdateDataRecord(dataRecord, model);
        }

        private bool CaseExists(AutoPopulateCaseModel model)
        {
            return _blaiseApi
                .WithConnection(_blaiseApi.DefaultConnection)
                .WithInstrument(model.InstrumentName)
                .WithServerPark(model.ServerPark)
                .Case
                .WithPrimaryKey(model.PrimaryKey)
                .Exists;
        }

        private IDataRecord GetCase(AutoPopulateCaseModel model)
        {
            return _blaiseApi
                .WithConnection(_blaiseApi.DefaultConnection)
                .WithInstrument(model.InstrumentName)
                .WithServerPark(model.ServerPark)
                .Case
                .WithPrimaryKey(model.PrimaryKey)
                .Get();
        }

        private void UpdateDataRecord(IDataRecord dataRecord, AutoPopulateCaseModel model)
        {
            _blaiseApi
                .WithConnection(_blaiseApi.DefaultConnection)
                .WithInstrument(model.InstrumentName)
                .WithServerPark(model.ServerPark)
                .Case
                .WithDataRecord(dataRecord)
                .WithData(model.Payload)
                .Update();
        }
    }
}