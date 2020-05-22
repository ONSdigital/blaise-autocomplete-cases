using BlaiseAutoCompleteCases.Interfaces.Mappers;
using BlaiseAutoCompleteCases.Models;
using BlaiseCaseReader.Interfaces.Mappers;
using Newtonsoft.Json;

namespace BlaiseAutoCompleteCases.Mappers
{
    public class ActionModelMapper : IActionModelMapper
    {
        public ActionModel MapToActionModel(string message)
        {
            return JsonConvert.DeserializeObject<ActionModel>(message);
        }
    }
}
