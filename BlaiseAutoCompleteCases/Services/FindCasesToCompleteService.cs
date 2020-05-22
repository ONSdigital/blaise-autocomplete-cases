using Blaise.Nuget.Api.Contracts.Interfaces;
using BlaiseAutoCompleteCases.Interfaces.Services;
using log4net;

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

        public void FindCasesToComplete(string instrumentName, int numberOfCasesToComplete)
        {
            throw new System.NotImplementedException();
        }
    }
}