using BlaiseCaseAutoComplete.Models;

namespace BlaiseCaseAutoComplete.Interfaces.Services
{
    public interface IAutoCompleteCasesService
    {
        void CompleteCases(AutoCompleteCaseModel model);
    }
}