using BlaiseAutoCompleteCases.Models;

namespace BlaiseAutoCompleteCases.Interfaces.Services
{
    public interface IPublishValidationRequestService
    {
        void PublishValidationRequest(ValidationModel validationModel);
    }
}