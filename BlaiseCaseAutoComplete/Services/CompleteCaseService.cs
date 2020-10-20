using Blaise.Nuget.Api.Contracts.Interfaces;
using BlaiseCaseAutoComplete.Helpers;
using BlaiseCaseAutoComplete.Interfaces.Services;
using BlaiseCaseAutoComplete.Models;
using log4net;
using StatNeth.Blaise.API.DataRecord;

namespace BlaiseCaseAutoComplete.Services
{
    public class CompleteCaseService : ICompleteCaseService
    {
        private readonly ILog _logger;
        private readonly IFluentBlaiseApi _blaiseApi;

        public CompleteCaseService(
            ILog logger,
            IFluentBlaiseApi blaiseApi)
        {
            _logger = logger;
            _blaiseApi = blaiseApi;
        }

        public void CompleteCase(AutoCompleteCaseModel model)
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
            CompleteCase(dataRecord, model);

            _logger.Info($"Populated case with primary key '{model.PrimaryKey}'");
        }

        public void CompleteCase(IDataRecord dataRecord, AutoCompleteCaseModel model)
        {
            model.InstrumentName.ThrowExceptionIfNullOrEmpty("InstrumentName");
            model.ServerPark.ThrowExceptionIfNullOrEmpty("ServerPark");

            UpdateDataRecord(dataRecord, model);
        }

        private bool CaseExists(AutoCompleteCaseModel model)
        {
            return _blaiseApi
                .WithConnection(_blaiseApi.DefaultConnection)
                .WithInstrument(model.InstrumentName)
                .WithServerPark(model.ServerPark)
                .Case
                .WithPrimaryKey(model.PrimaryKey)
                .Exists;
        }

        private IDataRecord GetCase(AutoCompleteCaseModel model)
        {
            return _blaiseApi
                .WithConnection(_blaiseApi.DefaultConnection)
                .WithInstrument(model.InstrumentName)
                .WithServerPark(model.ServerPark)
                .Case
                .WithPrimaryKey(model.PrimaryKey)
                .Get();
        }

        private void UpdateDataRecord(IDataRecord dataRecord, AutoCompleteCaseModel model)
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