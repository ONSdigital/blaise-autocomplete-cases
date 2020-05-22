using BlaiseAutoCompleteCases.Models;

namespace BlaiseAutoCompleteCases.Interfaces.Mappers
{
    public interface IValidationModelMapper
    {
        string MapToSerializedJson(ValidationModel validationModel);
    }
}