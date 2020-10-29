using System;
using BlaiseCaseAutoComplete.Interfaces.Mappers;
using BlaiseCaseAutoComplete.Interfaces.Services;
using BlaiseCaseAutoComplete.MessageHandler;
using BlaiseCaseAutoComplete.Models;
using log4net;
using Moq;
using NUnit.Framework;

namespace BlaiseCaseAutoComplete.Tests.MessageHandler
{
    public class AutoCompleteCaseHandlerTests
    {
        private Mock<ILog> _loggingMock;
        private Mock<IModelMapper> _mapperMock;
        private Mock<IAutoCompleteCasesService> _autoCompleteCasesServiceMock;
        private Mock<ICompleteCaseService> _completeCaseServiceMock;
        private Mock<IQueueService> _queueServiceMock;

        private readonly string _message;
        private readonly AutoCompleteCaseModel _caseModel;

        private AutoCompleteCaseHandler _sut;

        public AutoCompleteCaseHandlerTests()
        {
            _message = "Test message";
            _caseModel = new AutoCompleteCaseModel();
        }

        [SetUp]
        public void SetUpTests()
        {
            _loggingMock = new Mock<ILog>();

            _mapperMock = new Mock<IModelMapper>();
            _mapperMock.Setup(m => m.MapToModel(_message)).Returns(_caseModel);

            _autoCompleteCasesServiceMock = new Mock<IAutoCompleteCasesService>();

            _completeCaseServiceMock = new Mock<ICompleteCaseService>();

            _queueServiceMock = new Mock<IQueueService>();

            _sut = new AutoCompleteCaseHandler(
                _loggingMock.Object,
                _mapperMock.Object,
                _autoCompleteCasesServiceMock.Object,
                _completeCaseServiceMock.Object,
                _queueServiceMock.Object);
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
            _completeCaseServiceMock.Setup(c => c.CompleteCase(It.IsAny<AutoCompleteCaseModel>()));

            //act
            _sut.HandleMessage(_message);

            //assert
            _completeCaseServiceMock.Verify(v => v.CompleteCase(_caseModel), Times.Once);
            _autoCompleteCasesServiceMock.Verify(v => v.CompleteCases(It.IsAny<AutoCompleteCaseModel>()), Times.Never);
        }

        [Test]
        public void Given_A_Non_Specific_Case_Message_Is_Handled_Successfully_Then_The_Correct_Service_Is_Called()
        {
            //arrange
            _caseModel.PrimaryKey = null;
            _autoCompleteCasesServiceMock.Setup(c => c.CompleteCases(It.IsAny<AutoCompleteCaseModel>()));

            //act
            _sut.HandleMessage(_message);

            //assert
            _autoCompleteCasesServiceMock.Verify(v => v.CompleteCases(_caseModel), Times.Once);
            _completeCaseServiceMock.Verify(v => v.CompleteCase(It.IsAny<AutoCompleteCaseModel>()), Times.Never);
        }

        [Test]
        public void Given_An_Exception_When_I_Call_CompleteCases_The_Exception_Is_Handled_And_False_Returned()
        {
            //arrange
            _caseModel.PrimaryKey = null;
            var exceptionThrown = new Exception("Error raised");
            _autoCompleteCasesServiceMock.Setup(p => p.CompleteCases(It.IsAny<AutoCompleteCaseModel>())).Throws(exceptionThrown);

            //act
            var result = _sut.HandleMessage(_message);

            //assert
            Assert.IsFalse(result);
        }

        [Test]
        public void Given_An_Exception_When_I_Call_CompleteCase_The_Exception_Is_Handled_And_False_Returned()
        {
            //arrange
            _caseModel.PrimaryKey = "Key1";
            var exceptionThrown = new Exception("Error raised");
            _completeCaseServiceMock.Setup(p => p.CompleteCase(It.IsAny<AutoCompleteCaseModel>())).Throws(exceptionThrown);

            //act
            var result = _sut.HandleMessage(_message);

            //assert
            Assert.IsFalse(result);
        }
    }
}