using Blaise.Nuget.Api.Contracts.Interfaces;

namespace BlaiseAutoCompleteCases.Interfaces.Services
{
    public interface IBlaiseApiNew : IBlaiseApi
    {
        bool IsCaseComplete(string surveyName, string serverPark);
    }
}