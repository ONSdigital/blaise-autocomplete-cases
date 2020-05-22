using BlaiseAutoCompleteCases.Mappers;
using BlaiseAutoCompleteCases.Models;
using Newtonsoft.Json;
using NUnit.Framework;

namespace BlaiseAutoCompleteCases.Tests.Mappers
{
    public class ActionModelMapperTests
    {
        private readonly ActionModel _validationActionModel;

        private readonly ActionModelMapper _sut;

        public ActionModelMapperTests()
        {
            _validationActionModel = new ActionModel
            {
                action = "start_service_action"
            };

            _sut = new ActionModelMapper();
        }       

        [Test]
        public void Given_Valid_Input_When_I_Call_MapToActionModel_Then_I_Get_A_MapToActionModel_Returned()
        {
            //arrange
            var jsonMessage = JsonConvert.SerializeObject(_validationActionModel);

            //act
            var result = _sut.MapToActionModel(jsonMessage);

            //assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<ActionModel>(result);
        }

        [Test]
        public void Given_Valid_Input_When_I_Call_MapToActionModel_Then_I_Get_A_Correctly_Populated_MapToActionModel_Returned()
        {
            //arrange
            var jsonMessage = JsonConvert.SerializeObject(_validationActionModel);

            //act
            var result = _sut.MapToActionModel(jsonMessage);

            //assert
            Assert.AreEqual(_validationActionModel.action, result.action);
        }
    }
}
