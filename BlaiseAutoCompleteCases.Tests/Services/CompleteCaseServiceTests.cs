using Blaise.Nuget.Api.Contracts.Interfaces;
using BlaiseAutoCompleteCases.Services;
using log4net;
using Moq;
using NUnit.Framework;
using StatNeth.Blaise.API.ServerManager;
using System;
using System.Collections.Generic;
using BlaiseAutoCompleteCases.Interfaces.PersonData;
using StatNeth.Blaise.API.DataRecord;

namespace BlaiseAutoCompleteCases.Tests.Services
{
    public class CompleteCaseServiceTests
    {
        private CompleteCaseService _sut;
        private Mock<ILog> _loggingMock;
        private Mock<IBlaiseApi> _blaiseApiMock;
        private Mock<IPersonOutcome> _personOutcomeMock;
        private readonly string _instrumentName;
        private readonly string _serverParkName;

        public CompleteCaseServiceTests()
        {
            _instrumentName = "OPN2004A";
            _serverParkName = "TEL";
          

            var survey1Mock = new Mock<ISurvey>();
            survey1Mock.Setup(a => a.Name).Returns(_instrumentName);
            survey1Mock.Setup(a => a.ServerPark).Returns(_serverParkName);
        }

        [SetUp]
        public void SetUpTests()
        {
            _loggingMock = new Mock<ILog>();
            _blaiseApiMock = new Mock<IBlaiseApi>();
            _personOutcomeMock = new Mock<IPersonOutcome>();
            _sut = new CompleteCaseService(_loggingMock.Object, _blaiseApiMock.Object, _personOutcomeMock.Object);
        }

        [Test]
        public void Given_A_Null_InstrumentName_When_I_Call_CompleteCaseService_Then_A_ArgumentNullException_Is_Thrown()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.CompleteCase(It.IsAny<IDataRecord>(), null, _serverParkName));
            Assert.AreEqual("instrumentName", exception.ParamName);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_CompleteCaseService_Then_A_ArgumentNullException_Is_Thrown()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.CompleteCase(It.IsAny<IDataRecord>(), _instrumentName, null));
            Assert.AreEqual("serverParkName", exception.ParamName);
        }


        [Test]
        public void Given_An_Empty_InstrumentName_When_I_Call_CompleteCaseService_Then_A_ArgumentNullException_Is_Thrown()
        {
            var exception = Assert.Throws<ArgumentException>(() => _sut.CompleteCase(It.IsAny<IDataRecord>(), string.Empty, _serverParkName));
            Assert.AreEqual("A value for the argument 'instrumentName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_CompleteCaseService_Then_A_ArgumentNullException_Is_Thrown()
        {
            var exception = Assert.Throws<ArgumentException>(() => _sut.CompleteCase(It.IsAny<IDataRecord>(), _instrumentName, string.Empty));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Case_When_I_Call_CompleteCase_Then_A_Call_To_MarkCaseAsComplete_Is_Made()
        {
            //Act
            _sut.CompleteCase(It.IsAny<IDataRecord>(), _instrumentName, _serverParkName);

            //assert
            _blaiseApiMock.Verify(v => v.MarkCaseAsComplete(It.IsAny<IDataRecord>(), _instrumentName, _serverParkName), Times.Once);
        }

        [Test]
        public void Given_A_Case_When_I_Call_CompleteCase_Then_A_Call_To_UpdateDataRecord_Is_Made()
        {
            //Arrange
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            _personOutcomeMock.Setup(a => a.GetPersonOutcomeData_Good()).Returns(dictionary);

            //Act
            _sut.CompleteCase(It.IsAny<IDataRecord>(), _instrumentName, _serverParkName);

            //assert
            _blaiseApiMock.Verify(v => v.UpdateDataRecord(It.IsAny<IDataRecord>(), dictionary, _instrumentName, _serverParkName), Times.Once);
        }
    }
}
