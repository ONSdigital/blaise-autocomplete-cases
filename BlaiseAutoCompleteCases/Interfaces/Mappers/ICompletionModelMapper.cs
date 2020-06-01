using BlaiseCaseAutoComplete.Models;

namespace BlaiseCaseAutoComplete.Interfaces.Mappers
{
    public interface ICompletionModelMapper
    {
        string MapToSerializedJson(CompletionModel completionModel);
        CompletionModel MapToCompletionModel(string message);
    }
}