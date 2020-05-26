using System;
using System.Collections.Generic;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Blaise.Nuget.PubSub.Contracts.Interfaces;
using BlaiseAutoCompleteCases.Interfaces.Mappers;
using BlaiseAutoCompleteCases.Interfaces.Services;
using BlaiseAutoCompleteCases.Services;
using log4net;
using log4net.Core;
using Moq;
using NUnit.Framework;
using StatNeth.Blaise.API.DataLink;
using StatNeth.Blaise.API.DataRecord;
using StatNeth.Blaise.API.Meta;
using StatNeth.Blaise.API.ServerManager;

namespace BlaiseAutoCompleteCases.Tests.Services
{
    public class FindCasesToCompleteServiceTests
    {
        private FindCasesToCompleteService _sut;

        private Mock<ILog> _loggingMock;
        private Mock<IBlaiseApi> _blaiseApiMock;
        private Mock<IDatamodel> _dataModelMock;
        private Mock<IDataRecord2> _dataRecordMock;
        private Mock<IDataSet> _dataSetMock;
        private Mock<ICompleteCaseService> _completeCaseServiceMock;
        private Mock<IKeyCollection> _keyCollectionMock;
        private Mock<IFieldCollection> _fieldCollection;
        private Mock<IKey> _keyMock;
        private readonly List<ISurvey> _surveys;
        private readonly string _keyName;
        private readonly string _surveyName;
        private readonly string _serverParkName;
 

        public FindCasesToCompleteServiceTests()
        {
            _surveyName = "OPN2004A";
            _serverParkName = "TEL";
            _keyName = "PRIMARY";

            var survey1Mock = new Mock<ISurvey>();
            survey1Mock.Setup(a => a.Name).Returns(_surveyName);
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
            _dataModelMock = new Mock<IDatamodel>();
            _blaiseApiMock = new Mock<IBlaiseApi>();
            _dataRecordMock = new Mock<IDataRecord2>();
            _dataSetMock = new Mock<IDataSet>();
            _completeCaseServiceMock = new Mock<ICompleteCaseService>();
            _sut = new FindCasesToCompleteService(_loggingMock.Object, _blaiseApiMock.Object, _completeCaseServiceMock.Object);

        }

        [Test]
        public void Given_A_Null_SurveyName_When_I_Call_FindCasesToCompleteService_Then_A_ArgumentNullException_Is_Thrown()
        {
            //act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.FindCasesToComplete(null, 0));
            Assert.AreEqual("surveyName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_SurveyName_When_I_Call_FindCasesToCompleteService_Then_An_ArgumentException_Is_Thrown()
        {
            var exception = Assert.Throws<ArgumentException>(() => _sut.FindCasesToComplete(string.Empty, 0));
            Assert.AreEqual("A value for the argument 'surveyName' must be supplied", exception.Message);
        }

        [TestCase(-1)]
        [TestCase(0)]
        public void Given_An_Invalid_NumberOfCasesToComplete_When_I_Call_FindCasesToCompleteService_Then_An_ArgumentException_Is_Thrown(int numberOfCasesToComplete)
        {
            var exception = Assert.Throws<ArgumentException>(() => _sut.FindCasesToComplete(_surveyName, numberOfCasesToComplete));
            Assert.AreEqual("numberOfCasesToComplete", exception.Message);
        }

        [Test]
        public void Given_No_Instruments_Available_When_I_Call_FindCasesToCompleteService_No_Cases_Are_Processed()
        {
            //act
            _sut.FindCasesToComplete(_surveyName, 1);

            //assert
            _blaiseApiMock.Verify(v => v.GetDataSet(_surveyName, _serverParkName), Times.Never);
        }

        [Test]
        public void Given_All_Cases_Are_Already_Complete_When_I_Call_FindCasesToCompleteService_No_Cases_Are_Processed()
        {
            //arrange
            _blaiseApiMock.Setup(p => p.GetAllSurveys()).Returns(_surveys);
            _dataSetMock.SetupSequence(d => d.EndOfSet)
                .Returns(false)
                .Returns(true);

            _blaiseApiMock.Setup(d => d.GetDataSet(_surveyName, _serverParkName)).Returns(_dataSetMock.Object);
            _blaiseApiMock.Setup(s => s.CaseHasBeenCompleted(It.IsAny<IDataRecord>())).Returns(true);

            //act
            _sut.FindCasesToComplete(_surveyName, 1);

            //assert
            _blaiseApiMock.Verify(v => v.MarkCaseAsComplete(It.IsAny<IDataRecord>(), _surveyName, _serverParkName), Times.Never);
        }

        [Test]
        public void Given_All_Cases_Are_Already_Complete_When_I_Call_FindCasesToCompleteService_An_Appropriate_Message_Is_Logged()
        {
            //arrange
            _blaiseApiMock.Setup(p => p.GetAllSurveys()).Returns(_surveys);
            _dataSetMock.SetupSequence(d => d.EndOfSet)
                .Returns(false)
                .Returns(true);

            _blaiseApiMock.Setup(d => d.GetDataSet(_surveyName, _serverParkName)).Returns(_dataSetMock.Object);
            _blaiseApiMock.Setup(s => s.CaseHasBeenCompleted(It.IsAny<IDataRecord>())).Returns(true);

            //act
            _sut.FindCasesToComplete(_surveyName, 1);

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
            _dataSetMock.SetupSequence(d => d.EndOfSet)
                .Returns(false)
                .Returns(true);

            _dataSetMock.Setup(s => s.ActiveRecord).Returns(It.IsAny<IDataRecord>());

            _blaiseApiMock.Setup(d => d.GetDataSet(_surveyName, _serverParkName)).Returns(_dataSetMock.Object);
            _blaiseApiMock.Setup(s => s.CaseHasBeenCompleted(It.IsAny<IDataRecord>())).Returns(false);
            _blaiseApiMock.Setup(s => s.MarkCaseAsComplete(It.IsAny<IDataRecord>(), _surveyName, _serverParkName));

            //act
            _sut.FindCasesToComplete(_surveyName, casesToComplete);

            //assert
            _blaiseApiMock.Verify(v => v.MarkCaseAsComplete(It.IsAny<IDataRecord>(), _surveyName, _serverParkName), Times.Once);
        }

    }
}
