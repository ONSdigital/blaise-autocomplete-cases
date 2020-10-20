using BlaiseCaseAutoComplete.Models;

namespace BlaiseCaseAutoComplete.Interfaces.Services
{
    public interface IAutoPopulateCasesService
    {
        void PopulateCases(AutoCompleteCaseModel model);
    }
}