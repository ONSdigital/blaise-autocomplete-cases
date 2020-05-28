namespace BlaiseAutoCompleteCases.Interfaces.Services
{
    public interface ICompleteCasesService
    {
        void CompleteCases(string surveyName, int numberOfCasesToComplete);
    }
}