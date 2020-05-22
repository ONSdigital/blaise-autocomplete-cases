using BlaiseAutoCompleteCases.Enums;
using BlaiseAutoCompleteCases.Models;

namespace BlaiseAutoCompleteCases.Interfaces.Services
{
    public interface IProcessCompletionService
    {
        CompletionStatusType ProcessCompletion(ActionModel actionModel);
    }
}