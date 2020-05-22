namespace BlaiseAutoCompleteCases.Interfaces.Services
{
    public interface IFindCasesToCompleteService
    {
        void FindCasesToComplete(string surveyName, int numberOfCasesToComplete);
    }
}