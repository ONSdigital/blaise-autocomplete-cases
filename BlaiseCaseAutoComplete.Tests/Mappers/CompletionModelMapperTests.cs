using System.Collections.Generic;
using BlaiseCaseAutoComplete.Mappers;
using BlaiseCaseAutoComplete.Models;
using Newtonsoft.Json;
using NUnit.Framework;

namespace BlaiseCaseAutoComplete.Tests.Mappers
{
    public class CompletionModelMapperTests
    {
        private readonly AutoCompleteCaseModel _caseModel;
        private readonly ModelMapper _sut;

        public CompletionModelMapperTests()
        {
            _caseModel = new AutoCompleteCaseModel
            {
                CaseId = "54",
                InstrumentName = "OPN2000",
                ServerPark = "Park1",
                PrimaryKey = "Key1",
                NumberOfCases = 10,
                Payload = new Dictionary<string, string>
                {
                    {"Key1", "Value1"}
                }
            };

            _sut = new ModelMapper();
            
        }

        [Test]
        public void Given_No_Primary_Key_Is_Supplied_When_I_Call_MapToCompletionModel_Then_I_Get_A_Correctly_Populated_CompletionModel_Returned()
        {
            //arrange
            _caseModel.PrimaryKey = null;
            var jsonMessage = JsonConvert.SerializeObject(_caseModel);

            //act
            var result = _sut.MapToModel(jsonMessage);

            //assert
            Assert.AreEqual(_caseModel.NumberOfCases, result.NumberOfCases);
            Assert.AreEqual(_caseModel.CaseId, result.CaseId);
            Assert.AreEqual(_caseModel.InstrumentName, result.InstrumentName);
            Assert.AreEqual(_caseModel.ServerPark, result.ServerPark);
            Assert.AreEqual(_caseModel.Payload, result.Payload);

            Assert.IsNull(result.PrimaryKey);
            Assert.False(result.IsSpecificCase);
        }

        [Test]
        public void Given_A_Primary_Key_Is_Supplied_When_I_Call_MapToCompletionModel_Then_I_Get_A_Correctly_Populated_CompletionModel_Returned()
        {
            //arrange
            _caseModel.PrimaryKey = "Key1";
            var jsonMessage = JsonConvert.SerializeObject(_caseModel);

            //act
            var result = _sut.MapToModel(jsonMessage);

            //assert
            Assert.AreEqual(_caseModel.NumberOfCases, result.NumberOfCases);
            Assert.AreEqual(_caseModel.CaseId, result.CaseId);
            Assert.AreEqual(_caseModel.InstrumentName, result.InstrumentName);
            Assert.AreEqual(_caseModel.ServerPark, result.ServerPark);
            Assert.AreEqual(_caseModel.Payload, result.Payload);

            Assert.AreEqual(_caseModel.PrimaryKey, result.PrimaryKey);
            Assert.True(result.IsSpecificCase);
        }
    }
}