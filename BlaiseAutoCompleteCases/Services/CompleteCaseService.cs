using Blaise.Nuget.Api.Contracts.Interfaces;
using BlaiseAutoCompleteCases.Helpers;
using BlaiseAutoCompleteCases.Interfaces.Services;
using log4net;
using StatNeth.Blaise.API.DataRecord;


namespace BlaiseAutoCompleteCases.Services
{
    public class CompleteCaseService : ICompleteCaseService
    {
        private readonly ILog _logger;
        private readonly IBlaiseApi _blaiseApi;

        public CompleteCaseService(ILog logger, IBlaiseApi blaiseApi)
        {
            _logger = logger;
            _blaiseApi = blaiseApi;
        }

        public void CompleteCase(IDataRecord dataRecord, string instrumentName, string serverParkName)
        {
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            _blaiseApi.MarkCaseAsComplete(dataRecord, instrumentName, serverParkName);
        }
    }
}