using BlaiseAutoCompleteCases.Interfaces.Mappers;
using BlaiseAutoCompleteCases.Models;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace BlaiseAutoCompleteCases.Mappers
{
    public class ValidationModelMapper : IValidationModelMapper
    {
        public string MapToSerializedJson(ValidationModel validationModel)
        {
            var validationJson = new Dictionary<string, dynamic>
            {
                ["primary_key"] = validationModel.primary_key,
                ["rules_id"] = validationModel.rules_id,
                ["data"] = validationModel.data_fields
            };

            //Add QID fields by iterating through
            foreach (KeyValuePair<string, dynamic> kvp in validationModel.qid_fields)
            {
                validationJson.Add(kvp.Key, kvp.Value);
            }

            return JsonConvert.SerializeObject(validationJson);
        }
    }
}
