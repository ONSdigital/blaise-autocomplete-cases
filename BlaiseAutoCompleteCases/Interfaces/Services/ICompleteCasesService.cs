namespace BlaiseCaseAutoComplete.Interfaces.Services
{
    public interface ICompleteCasesService
    {
        void CompleteCases(string surveyName, int numberOfCasesToComplete);
    }
}