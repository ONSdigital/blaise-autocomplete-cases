using BlaiseAutoCompleteCases.Enums;
using BlaiseAutoCompleteCases.Interfaces.Mappers;
using BlaiseAutoCompleteCases.Interfaces.Services;
using BlaiseAutoCompleteCases.MessageHandler;
using BlaiseAutoCompleteCases.Models;
using log4net;
using Moq;
using NUnit.Framework;
using System;
using BlaiseCaseReader.Interfaces.Mappers;

namespace BlaiseAutoCompleteCases.Tests.MessageHandler
{
    public class BlaiseCaseActionHandlerTests
    {
        private Mock<ILog> _loggingMock;
        private Mock<IActionModelMapper> _mapperMock;
        private Mock<IProcessCompletionService> _processCaseServiceMock;
        private Mock<IPublishValidationRequestService> _publishValidationRequestServiceMock;

        private readonly string _message;
        private readonly ActionModel _caseReaderModel;
        private readonly ValidationModel _validationModel;
        private readonly CompletionStatusType _completionStatusType;
        private BlaiseCaseActionHandler _sut;

        public BlaiseCaseActionHandlerTests()
        {
            _message = "Test message";
            _completionStatusType = CompletionStatusType.CaseAutoCompleted;
            _caseReaderModel = new ActionModel();
            _validationModel = new ValidationModel();
        }

        [SetUp]
        public void SetUpTests()
        {
            _loggingMock = new Mock<ILog>();
            
            _mapperMock = new Mock<IActionModelMapper>();
            _mapperMock.Setup(m => m.MapToActionModel(_message)).Returns(_caseReaderModel);

            _processCaseServiceMock = new Mock<IProcessCompletionService>();
            _processCaseServiceMock.Setup(p => p.ProcessCompletion(_caseReaderModel)).Returns(_completionStatusType);

            _publishValidationRequestServiceMock = new Mock<IPublishValidationRequestService>();
            _publishValidationRequestServiceMock.Setup(p => p.PublishValidationRequest(_validationModel));

            _sut = new BlaiseCaseActionHandler(
                    _loggingMock.Object,
                    _mapperMock.Object,
                    _processCaseServiceMock.Object);
        }

        [Test]
        public void Given_A_Message_Is_Handled_Successfully_Then_True_Is_Returned()
        {
            //act
            var result = _sut.HandleMessage(_message);

            //assert
            Assert.IsTrue(result);
        }

        [Test]
        public void Given_A_Message_Is_Not_Handled_Successfully_Then_False_Is_Returned()
        {
            //arrange
            var exceptionThrown = new Exception("Error message");
            _processCaseServiceMock.Setup(p => p.ProcessCompletion(It.IsAny<ActionModel>())).Throws(exceptionThrown);

            //act
            var result = _sut.HandleMessage(_message);

            //assert
            Assert.IsFalse(result);
        }

        [Test]
        public void Given_A_Message_Is_Not_Handled_Successfully_Then_The_Exception_Is_Logged()
        {
            //arrange
            var exceptionThrown = new Exception("Error message");
            _processCaseServiceMock.Setup(p => p.ProcessCompletion(It.IsAny<ActionModel>())).Throws(exceptionThrown);

            //act
            _sut.HandleMessage(_message);

            //assert
            _loggingMock.Verify(v => v.Error(exceptionThrown), Times.Once);
        }

        [Test]
        public void Given_A_Message_Is_Handled_Successfully_Then_The_Case_Is_Processed()
        {
            //act
            _sut.HandleMessage(_message);

            //assert
            _processCaseServiceMock.Verify(v => v.ProcessCompletion(_caseReaderModel), Times.Once);
        }
    }
}
