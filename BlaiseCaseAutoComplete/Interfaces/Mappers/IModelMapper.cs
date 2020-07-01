using BlaiseCaseAutoComplete.Models;

namespace BlaiseCaseAutoComplete.Interfaces.Mappers
{
    public interface IModelMapper
    {
       AutoCompleteCaseModel MapToModel(string message);
    }
}