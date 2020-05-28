using Blaise.Nuget.Api.Contracts.Interfaces;
using BlaiseAutoCompleteCases.Interfaces.Services;
using BlaiseAutoCompleteCases.Services;
using log4net;
using Moq;
using NUnit.Framework;
using StatNeth.Blaise.API.DataLink;
using StatNeth.Blaise.API.DataRecord;
using StatNeth.Blaise.API.ServerManager;
using System;
using System.Collections.Generic;

namespace BlaiseAutoCompleteCases.Tests.Services
{
    public class CompleteCasesServiceTests
    {
        private CompleteCasesService _sut;

        private Mock<ILog> _loggingMock;
        private Mock<IBlaiseApi> _blaiseApiMock;
        private Mock<IDataSet> _dataSetMock;
        private Mock<ICompleteCaseService> _completeCaseServiceMock;
        private readonly List<ISurvey> _surveys;
        private readonly string _instrumentName;
        private readonly string _serverParkName;
 

        public CompleteCasesServiceTests()
        {
            _instrumentName = "OPN2004A";
            _serverParkName = "TEL";

            var survey1Mock = new Mock<ISurvey>();
            survey1Mock.Setup(a => a.Name).Returns(_instrumentName);
            survey1Mock.Setup(a => a.ServerPark).Returns(_serverParkName);

            //var survey2Mock = new Mock<ISurvey>();
            //survey2Mock.Setup(a => a.Name).Returns(_surveyName);
            //survey2Mock.Setup(a => a.ServerPark).Returns(_serverParkName);

            _surveys = new List<ISurvey> { survey1Mock.Object};
        }

        [SetUp]
        public void SetUpTests()
        {
            _loggingMock = new Mock<ILog>();
            _blaiseApiMock = new Mock<IBlaiseApi>();
            _dataSetMock = new Mock<IDataSet>();
            _completeCaseServiceMock = new Mock<ICompleteCaseService>();
            _sut = new CompleteCasesService(_loggingMock.Object, _blaiseApiMock.Object, _completeCaseServiceMock.Object);

        }

        [Test]
        public void Given_A_Null_SurveyName_When_I_Call_FindCasesToCompleteService_Then_A_ArgumentNullException_Is_Thrown()
        {
            //act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.CompleteCases(null, 0));
            Assert.AreEqual("surveyName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_SurveyName_When_I_Call_FindCasesToCompleteService_Then_An_ArgumentException_Is_Thrown()
        {
            var exception = Assert.Throws<ArgumentException>(() => _sut.CompleteCases(string.Empty, 0));
            Assert.AreEqual("A value for the argument 'surveyName' must be supplied", exception.Message);
        }

        [TestCase(-1)]
        [TestCase(0)]
        public void Given_An_Invalid_NumberOfCasesToComplete_When_I_Call_FindCasesToCompleteService_Then_An_ArgumentException_Is_Thrown(int numberOfCasesToComplete)
        {
            var exception = Assert.Throws<ArgumentException>(() => _sut.CompleteCases(_instrumentName, numberOfCasesToComplete));
            Assert.AreEqual("numberOfCasesToComplete", exception.Message);
        }

        [Test]
        public void Given_No_Instruments_Available_When_I_Call_FindCasesToCompleteService_No_Cases_Are_Processed()
        {
            //act
            _sut.CompleteCases(_instrumentName, 1);

            //assert
            _blaiseApiMock.Verify(v => v.GetDataSet(_instrumentName, _serverParkName), Times.Never);
        }

        [Test]
        public void Given_All_Cases_Are_Already_Complete_When_I_Call_FindCasesToCompleteService_No_Cases_Are_Processed()
        {
            //arrange
            _blaiseApiMock.Setup(p => p.GetAllSurveys()).Returns(_surveys);
            _dataSetMock.SetupSequence(d => d.EndOfSet)
                .Returns(false)
                .Returns(true);

            _blaiseApiMock.Setup(d => d.GetDataSet(_instrumentName, _serverParkName)).Returns(_dataSetMock.Object);
            _blaiseApiMock.Setup(s => s.CaseHasBeenCompleted(It.IsAny<IDataRecord>())).Returns(true);

            //act
            _sut.CompleteCases(_instrumentName, 1);

            //assert
            _blaiseApiMock.Verify(v => v.MarkCaseAsComplete(It.IsAny<IDataRecord>(), _instrumentName, _serverParkName), Times.Never);
        }

        [Test]
        public void Given_All_Cases_Are_Already_Complete_When_I_Call_FindCasesToCompleteService_An_Appropriate_Message_Is_Logged()
        {
            //arrange
            _blaiseApiMock.Setup(p => p.GetAllSurveys()).Returns(_surveys);
            _dataSetMock.SetupSequence(d => d.EndOfSet)
                .Returns(false)
                .Returns(true);

            _blaiseApiMock.Setup(d => d.GetDataSet(_instrumentName, _serverParkName)).Returns(_dataSetMock.Object);
            _blaiseApiMock.Setup(s => s.CaseHasBeenCompleted(It.IsAny<IDataRecord>())).Returns(true);

            //act
            _sut.CompleteCases(_instrumentName, 1);

            //assert
            _loggingMock.Verify(v => v.Info("No Cases Found to Complete"), Times.Once);
        }

        [TestCase(1, 1)]
        [TestCase(10, 10)]
        [TestCase(10, 5)]
        [TestCase(5, 10)]
        public void Given_X_Cases_To_Complete_And_Y_Cases_Available_When_I_Call_FindCasesToCompleteService_Then_The_Appropriate_Cases_Are_Completed(int casesToComplete, int casesAvailable)
        {
            //arrange
            _blaiseApiMock.Setup(p => p.GetAllSurveys()).Returns(_surveys);

            var sequence = _dataSetMock.SetupSequence(c => c.EndOfSet);
            for (int i = 0; i < casesAvailable; i++)
            {
                sequence.Returns(false);
            }
            sequence.Returns(true);

            _dataSetMock.Setup(s => s.ActiveRecord).Returns(It.IsAny<IDataRecord>());

            _blaiseApiMock.Setup(d => d.GetDataSet(_instrumentName, _serverParkName)).Returns(_dataSetMock.Object);
            _blaiseApiMock.Setup(s => s.CaseHasBeenCompleted(It.IsAny<IDataRecord>())).Returns(false);
            _blaiseApiMock.Setup(s => s.MarkCaseAsComplete(It.IsAny<IDataRecord>(), _instrumentName, _serverParkName));

            //act
            _sut.CompleteCases(_instrumentName, casesToComplete);

            //assert
            _completeCaseServiceMock.Verify(v => v.CompleteCase(It.IsAny<IDataRecord>(), _instrumentName, _serverParkName), Times.Exactly(casesAvailable));
        }

    }
}
