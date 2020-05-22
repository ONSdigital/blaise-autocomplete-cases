using BlaiseAutoCompleteCases.Models;
using BlaiseAutoCompleteCases.Models;

namespace BlaiseCaseReader.Interfaces.Mappers
{
    public interface IActionModelMapper
    {
        ActionModel MapToActionModel(string message);
    }
}