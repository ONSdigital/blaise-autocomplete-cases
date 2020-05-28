using BlaiseAutoCompleteCases.Models;

namespace BlaiseAutoCompleteCases.Interfaces.Mappers
{
    public interface ICompletionModelMapper
    {
        string MapToSerializedJson(CompletionModel completionModel);
        CompletionModel MapToCompletionModel(string message);
    }
}