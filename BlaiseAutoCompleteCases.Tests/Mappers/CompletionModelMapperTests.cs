using BlaiseAutoCompleteCases.Mappers;
using BlaiseAutoCompleteCases.Models;
using Newtonsoft.Json;
using NUnit.Framework;

namespace BlaiseAutoCompleteCases.Tests.Mappers
{
    public class CompletionModelMapperTests
    {
        private readonly CompletionModel _completionModel;
        private readonly CompletionModelMapper _sut;

        public CompletionModelMapperTests()
        {
            _completionModel = new CompletionModel
            {
                SurveyName = "OPN2004A",
                NumberOfCasesToComplete = 1
            };

            _sut = new CompletionModelMapper();
            
        }

        [Test]
        public void When_I_Call_MapToCompletionModel_A_Valid_Model_Is_Returned()
        {
            //arrange
            var jsonMessage = JsonConvert.SerializeObject(_completionModel);

            //act
            var result = _sut.MapToCompletionModel(jsonMessage);

            //assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<CompletionModel>(result);
        }

        [Test]
        public void Given_Valid_Input_When_I_Call_MapToCompletionModel_Then_I_Get_A_Correctly_Populated_CompletionModel_Returned()
        {
            //arrange
            var jsonMessage = JsonConvert.SerializeObject(_completionModel);

            //act
            var result = _sut.MapToCompletionModel(jsonMessage);

            //assert
            Assert.AreEqual(_completionModel.NumberOfCasesToComplete, result.NumberOfCasesToComplete);
            Assert.AreEqual(_completionModel.SurveyName, result.SurveyName);
        }

        [Test]
        public void When_I_Call_MapToSerializedJson_A_Valid_Model_Is_Returned()
        {
            //arrange & act
            var result = _sut.MapToSerializedJson(_completionModel);

            //assert
            Assert.NotNull(result);
            Assert.AreEqual("{\"survey_name\":\"OPN2004A\",\"number_of_cases_to_complete\":\"1\"}", result);
        }
    }
}