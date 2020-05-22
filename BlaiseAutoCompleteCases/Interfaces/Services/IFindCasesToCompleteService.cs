namespace BlaiseAutoCompleteCases.Interfaces.Services
{
    public interface IFindCasesToCompleteService
    {
        void FindCasesToComplete(string instrumentName, int numberOfCasesToComplete);
    }
}