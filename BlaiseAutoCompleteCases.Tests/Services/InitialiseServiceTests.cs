using Blaise.Nuget.PubSub.Contracts.Interfaces;
using BlaiseAutoCompleteCases.Interfaces.Services;
using BlaiseAutoCompleteCases.Services;
using log4net;
using Moq;
using NUnit.Framework;
using System;

namespace BlaiseAutoCompleteCases.Tests.Services
{
    public class InitialiseServiceTests
    {
        private Mock<ILog> _loggingMock;
        private Mock<IQueueService> _queueServiceMock;
        private Mock<IMessageHandler> _messageHandlerMock;

        private InitialiseService _sut;

        [SetUp]
        public void SetUpTests()
        {
            _loggingMock = new Mock<ILog>();
            _queueServiceMock = new Mock<IQueueService>();
            _messageHandlerMock = new Mock<IMessageHandler>();

            _sut = new InitialiseService(
                _loggingMock.Object,
                _queueServiceMock.Object,
                _messageHandlerMock.Object);
        }

        [Test]
        public void Given_I_Call_Start_Then_The_Correct_Methods_Are_Called_To_Setup_And_Subscribe_To_The_Appropriate_Queues()
        {
            //arrange

            //act
            _sut.Start();

            //assert
            _queueServiceMock.Verify(v => v.Subscribe(_messageHandlerMock.Object), Times.Once);
        }

        [Test]
        public void Given_I_Call_Start_And_An_Exception_Is_Thrown_During_The_Process_Then_The_Exception_Is_Handled()
        {
            //arrange
            var exceptionThrown = new Exception("Error message");
            _queueServiceMock.Setup(p => p.Subscribe(It.IsAny<IMessageHandler>())).Throws(exceptionThrown);
            _loggingMock.Setup(l => l.Error(It.IsAny<Exception>()));

            //act && assert
            Assert.DoesNotThrow(() => _sut.Start());
        }

        [Test]
        public void Given_I_Call_Start_And_An_Exception_Is_Thrown_During_The_Process_Then_The_Exception_Is_Logged()
        {
            //arrange
            var exceptionThrown = new Exception("Error message");
            _queueServiceMock.Setup(p => p.Subscribe(It.IsAny<IMessageHandler>())).Throws(exceptionThrown);
            _loggingMock.Setup(l => l.Error(It.IsAny<Exception>()));

            //act
            _sut.Start();

            //assert
            _loggingMock.Verify(v => v.Error(exceptionThrown), Times.Once);
        }

        [Test]
        public void Given_I_Call_Stop_Then_The_Appropriate_Service_Is_Called()
        {
            //act
            _sut.Stop();

            //assert
            _queueServiceMock.Verify(v => v.CancelAllSubscriptions(), Times.Once);
        }
    }
}
