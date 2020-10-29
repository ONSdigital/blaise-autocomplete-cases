using Blaise.Case.Auto.Populate.Interfaces.Mappers;
using Blaise.Case.Auto.Populate.Models;
using Newtonsoft.Json;

namespace Blaise.Case.Auto.Populate.Mappers
{
    public class ModelMapper : IModelMapper
    {
        public AutoPopulateCaseModel MapToModel(string message)
        {
            return JsonConvert.DeserializeObject<AutoPopulateCaseModel>(message);
        }
    }
}