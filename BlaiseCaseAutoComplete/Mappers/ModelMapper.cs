using BlaiseCaseAutoComplete.Interfaces.Mappers;
using BlaiseCaseAutoComplete.Models;
using Newtonsoft.Json;

namespace BlaiseCaseAutoComplete.Mappers
{
    public class ModelMapper : IModelMapper
    {
        public AutoCompleteCaseModel MapToModel(string message)
        {
            return JsonConvert.DeserializeObject<AutoCompleteCaseModel>(message);
        }
    }
}