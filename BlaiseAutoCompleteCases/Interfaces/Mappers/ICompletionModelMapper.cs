using BlaiseAutoCompleteCases.Models;

namespace BlaiseAutoCompleteCases.Interfaces.Mappers
{
    public interface ICompletionModelMapper
    {
        CompletionModel MapToCompletionModel(string message);
    }
}