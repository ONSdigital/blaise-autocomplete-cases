using Blaise.Nuget.Api.Contracts.Interfaces;
using BlaiseAutoCompleteCases.Helpers;
using BlaiseAutoCompleteCases.Interfaces.PersonData;
using BlaiseAutoCompleteCases.Interfaces.Services;
using log4net;
using StatNeth.Blaise.API.DataRecord;


namespace BlaiseAutoCompleteCases.Services
{
    public class CompleteCaseService : ICompleteCaseService
    {
        private readonly ILog _logger;
        private readonly IBlaiseApi _blaiseApi;
        private readonly IPersonOutcome _personOutCome;

        public CompleteCaseService(ILog logger, IBlaiseApi blaiseApi, IPersonOutcome personOutcome)
        {
            _logger = logger;
            _blaiseApi = blaiseApi;
            _personOutCome = personOutcome;
        }

        public void CompleteCase(IDataRecord dataRecord, string instrumentName, string serverParkName)
        {
            _logger.Info($"Setting case as complete, Instrument Name: {instrumentName}, ServeParkName: {serverParkName}");

            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");
            _blaiseApi.UpdateDataRecord(dataRecord, _personOutCome.GetPersonOutcomeData_Good(), instrumentName, serverParkName);
            _blaiseApi.MarkCaseAsComplete(dataRecord, instrumentName, serverParkName);
        }
    }
}