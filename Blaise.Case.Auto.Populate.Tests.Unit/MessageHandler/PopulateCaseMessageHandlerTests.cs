using System;
using Blaise.Case.Auto.Populate.Interfaces.Mappers;
using Blaise.Case.Auto.Populate.Interfaces.Services;
using Blaise.Case.Auto.Populate.MessageHandler;
using Blaise.Case.Auto.Populate.Models;
using log4net;
using Moq;
using NUnit.Framework;

namespace Blaise.Case.Auto.Populate.Tests.Unit.MessageHandler
{
    public class PopulateCaseMessageHandlerTests
    {
        private Mock<ILog> _loggingMock;
        private Mock<IModelMapper> _mapperMock;
        private Mock<IPopulateCasesService> _populateCasesServiceMock;
        private Mock<IPopulateCaseService> _populateCaseServiceMock;

        private readonly string _message;
        private readonly AutoPopulateCaseModel _caseModel;

        private PopulateCaseMessageHandler _sut;

        public PopulateCaseMessageHandlerTests()
        {
            _message = "Test message";
            _caseModel = new AutoPopulateCaseModel();
        }

        [SetUp]
        public void SetUpTests()
        {
            _loggingMock = new Mock<ILog>();

            _mapperMock = new Mock<IModelMapper>();
            _mapperMock.Setup(m => m.MapToModel(_message)).Returns(_caseModel);

            _populateCasesServiceMock = new Mock<IPopulateCasesService>();

            _populateCaseServiceMock = new Mock<IPopulateCaseService>();

            _sut = new PopulateCaseMessageHandler(
                _loggingMock.Object,
                _mapperMock.Object,
                _populateCasesServiceMock.Object,
                _populateCaseServiceMock.Object);
        }

        [Test]
        public void Given_A_Specific_Case_Message_Is_Handled_Successfully_Then_True_Is_Returned()
        {
            //arrange
            _caseModel.PrimaryKey = "Key1";

            //act
            var result = _sut.HandleMessage(_message);

            //assert
            Assert.IsTrue(result);
        }

        [Test]
        public void Given_A_Non_Specific_Case_Message_Is_Handled_Successfully_Then_True_Is_Returned()
        {
            //arrange
            _caseModel.PrimaryKey = null;

            //act
            var result = _sut.HandleMessage(_message);

            //assert
            Assert.IsTrue(result);
        }

        [Test]
        public void Given_A_Specific_Case_Message_Is_Handled_Successfully_Then_The_Correct_Service_Is_Called()
        {
            //arrange
            _caseModel.PrimaryKey = "Key1";
            _populateCaseServiceMock.Setup(c => c.PopulateCase(It.IsAny<AutoPopulateCaseModel>()));

            //act
            _sut.HandleMessage(_message);

            //assert
            _populateCaseServiceMock.Verify(v => v.PopulateCase(_caseModel), Times.Once);
            _populateCasesServiceMock.Verify(v => v.PopulateCases(It.IsAny<AutoPopulateCaseModel>()), Times.Never);
        }

        [Test]
        public void Given_A_Non_Specific_Case_Message_Is_Handled_Successfully_Then_The_Correct_Service_Is_Called()
        {
            //arrange
            _caseModel.PrimaryKey = null;
            _populateCasesServiceMock.Setup(c => c.PopulateCases(It.IsAny<AutoPopulateCaseModel>()));

            //act
            _sut.HandleMessage(_message);

            //assert
            _populateCasesServiceMock.Verify(v => v.PopulateCases(_caseModel), Times.Once);
            _populateCaseServiceMock.Verify(v => v.PopulateCase(It.IsAny<AutoPopulateCaseModel>()), Times.Never);
        }

        [Test]
        public void Given_An_Exception_When_I_Call_PopulateCases_The_Exception_Is_Handled_And_False_Returned()
        {
            //arrange
            _caseModel.PrimaryKey = null;
            var exceptionThrown = new Exception("Error raised");
            _populateCasesServiceMock.Setup(p => p.PopulateCases(It.IsAny<AutoPopulateCaseModel>())).Throws(exceptionThrown);

            //act
            var result = _sut.HandleMessage(_message);

            //assert
            Assert.IsFalse(result);
        }

        [Test]
        public void Given_An_Exception_When_I_Call_PopulateCase_The_Exception_Is_Handled_And_False_Returned()
        {
            //arrange
            _caseModel.PrimaryKey = "Key1";
            var exceptionThrown = new Exception("Error raised");
            _populateCaseServiceMock.Setup(p => p.PopulateCase(It.IsAny<AutoPopulateCaseModel>())).Throws(exceptionThrown);

            //act
            var result = _sut.HandleMessage(_message);

            //assert
            Assert.IsFalse(result);
        }
    }
}