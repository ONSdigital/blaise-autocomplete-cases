using System;
using Blaise.Nuget.Api.Contracts.Interfaces;
using BlaiseAutoCompleteCases.Interfaces.Mappers;
using BlaiseAutoCompleteCases.Interfaces.Services;
using BlaiseAutoCompleteCases.MessageHandler;
using BlaiseAutoCompleteCases.Models;
using log4net;
using Moq;
using NUnit.Framework;

namespace BlaiseAutoCompleteCases.Tests.MessageHandler
{
    public class CaseCompletionHandlerTests
    {
        private Mock<ILog> _loggingMock;
        private Mock<ICompletionModelMapper> _completionModelMapperMock;
        private Mock<ICompleteCasesService> _findCasesToCompleteServiceMock;
        private Mock<IBlaiseApi> _blaiseApiMock;

        private readonly string _message;
        private readonly CompletionModel _completionModel;

        private CaseCompletionHandler _sut;

        public CaseCompletionHandlerTests()
        {
            _message = "Test message";
            _completionModel = new CompletionModel();
        }

        [SetUp]
        public void SetUpTests()
        {
            _loggingMock = new Mock<ILog>();

            _completionModelMapperMock = new Mock<ICompletionModelMapper>();
            _completionModelMapperMock.Setup(m => m.MapToCompletionModel(_message)).Returns(_completionModel);

            _findCasesToCompleteServiceMock = new Mock<ICompleteCasesService>();
            _findCasesToCompleteServiceMock.Setup(p =>
                p.CompleteCases(It.IsAny<string>(), It.IsAny<int>()));

            _blaiseApiMock = new Mock<IBlaiseApi>();
            _blaiseApiMock.Setup(p => p.ServerParkExists(It.IsAny<string>())).Returns(true);

            _sut = new CaseCompletionHandler(
                _loggingMock.Object,
                _completionModelMapperMock.Object,
                _findCasesToCompleteServiceMock.Object);
        }

        [Test]
        public void Given_A_Message_Is_Handled_Successfully_Then_True_Is_Returned()
        {
            //arrange

            //act
            var result = _sut.HandleMessage(_message);

            //assert
            Assert.IsTrue(result);
        }

        [Test]
        public void Given_An_Exception_When_I_Call_CompleteCases_The_Exception_Is_Handled()
        {
            //arrange
            var exceptionThrown = new Exception("Error raised");
            _findCasesToCompleteServiceMock.Setup(p => p.CompleteCases(It.IsAny<string>(), It.IsAny<int>())).Throws(exceptionThrown);

            //act
            var result = _sut.HandleMessage(_message);

            //assert
            Assert.IsFalse(result);
        }

        [Test]
        public void Given_A_Message_Is_Not_Handled_Successfully_When_I_Call_CompleteCases_Then_The_Exception_Is_Logged()
        {
            //arrange
            var exceptionThrown = new Exception("Error message");
            _findCasesToCompleteServiceMock.Setup(p => p.CompleteCases(It.IsAny<string>(), It.IsAny<int>())).Throws(exceptionThrown);

            //act
            _sut.HandleMessage(_message);

            //assert
            _loggingMock.Verify(v => v.Error(exceptionThrown), Times.Once);
        }

        [Test]
        public void Given_A_Message_Is_Handled_Successfully_When_I_Call_CompleteCases_Then_Cases_Are_Completed()
        {
            //act
            _sut.HandleMessage(_message);

            //assert
            _findCasesToCompleteServiceMock.Verify(v => v.CompleteCases(It.IsAny<string>(), It.IsAny<int>()),
                Times.Once);
        }

        [Test]
        public void Given_An_Exception_When_I_Call_MapCompletionToModel_Then_The_Exception_Is_Handled()
        {
            //arrange
            var exceptionThrown = new Exception("Error raised");
            _completionModelMapperMock.Setup(p => p.MapToCompletionModel(It.IsAny<string>())).Throws(exceptionThrown);

            //act
            var result = _sut.HandleMessage(_message);

            //assert
            Assert.IsFalse(result);
        }

        [Test]
        public void Given_A_Message_Is_Not_Handled_Successfully_When_I_Call_MapCompletionToModel_Then_The_Exception_Is_Logged()
        {
            //arrange
            var exceptionThrown = new Exception("Error message");
            _completionModelMapperMock.Setup(p => p.MapToCompletionModel(It.IsAny<string>())).Throws(exceptionThrown);

            //act
            _sut.HandleMessage(_message);

            //assert
            _loggingMock.Verify(v => v.Error(exceptionThrown), Times.Once);
        }

        [Test]
        public void Given_A_Message_Is_Handled_Successfully_When_I_Call_MapCompletionToModel_Then_Completion_Is_Mapped()
        {
            //act
            _sut.HandleMessage(_message);

            //assert
            _completionModelMapperMock.Verify(v => v.MapToCompletionModel(It.IsAny<string>()),Times.Once);
        }
    }
}