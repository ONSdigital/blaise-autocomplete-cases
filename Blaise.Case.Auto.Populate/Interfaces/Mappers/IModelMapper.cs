using Blaise.Case.Auto.Populate.Models;

namespace Blaise.Case.Auto.Populate.Interfaces.Mappers
{
    public interface IModelMapper
    {
       AutoPopulateCaseModel MapToModel(string message);
    }
}