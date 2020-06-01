using System.Collections.Generic;
using BlaiseCaseAutoComplete.Interfaces.Mappers;
using BlaiseCaseAutoComplete.Models;
using Newtonsoft.Json;

namespace BlaiseCaseAutoComplete.Mappers
{
    public class CompletionModelMapper : ICompletionModelMapper
    {
        public string MapToSerializedJson(CompletionModel completionModel)
        {
            var jsonData = new Dictionary<string, string>
            {
                ["survey_name"] = completionModel.SurveyName,
                ["number_of_cases_to_complete"] = completionModel.NumberOfCasesToComplete.ToString()
            };

            return JsonConvert.SerializeObject(jsonData);
        }

        public CompletionModel MapToCompletionModel(string message)
        {
            return JsonConvert.DeserializeObject<CompletionModel>(message);
        }
    }
}