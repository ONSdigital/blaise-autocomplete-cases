using System;
using System.Collections.Generic;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Blaise.Nuget.Api.Contracts.Models;
using BlaiseCaseAutoComplete.Interfaces.Services;
using BlaiseCaseAutoComplete.Models;
using BlaiseCaseAutoComplete.Services;
using log4net;
using Moq;
using NUnit.Framework;
using StatNeth.Blaise.API.DataLink;
using StatNeth.Blaise.API.DataRecord;

namespace BlaiseCaseAutoComplete.Tests.Services
{
    public class AutoCompleteCasesServiceTests
    {
        private AutoCompleteCasesService _sut;

        private Mock<ILog> _loggingMock;
        private Mock<IFluentBlaiseApi> _blaiseApiMock;
        private Mock<IDataSet> _dataSetMock;
        private Mock<ICompleteCaseService> _completeCaseServiceMock;

        private readonly string _instrumentName;
        private readonly string _serverParkName;

        private AutoCompleteCaseModel _caseModel;

        public AutoCompleteCasesServiceTests()
        {
            _instrumentName = "OPN2004A";
            _serverParkName = "TEL";

        }

        [SetUp]
        public void SetUpTests()
        {
            _caseModel = new AutoCompleteCaseModel
            {
                InstrumentName = _instrumentName,
                ServerPark = _serverParkName,
                NumberOfCases = 10,
                Payload = new Dictionary<string, string>
                {
                    {"Key1", "Value1"}
                }
            };

            _loggingMock = new Mock<ILog>();
            
            _blaiseApiMock = new Mock<IFluentBlaiseApi>();
            _blaiseApiMock.Setup(b => b.WithConnection(It.IsAny<ConnectionModel>())).Returns(_blaiseApiMock.Object);
            _blaiseApiMock.Setup(b => b.WithInstrument(_instrumentName)).Returns(_blaiseApiMock.Object);
            _blaiseApiMock.Setup(b => b.WithServerPark(_serverParkName)).Returns(_blaiseApiMock.Object);
            _blaiseApiMock.Setup(b => b.Survey.Exists).Returns(true);

            _dataSetMock = new Mock<IDataSet>();
            _completeCaseServiceMock = new Mock<ICompleteCaseService>();
            _sut = new AutoCompleteCasesService(_loggingMock.Object, _blaiseApiMock.Object, _completeCaseServiceMock.Object);
        }

        [Test]
        public void Given_A_Null_instrumentName_When_I_Call_CompleteCases_Then_A_ArgumentNullException_Is_Thrown()
        {
            //arrange
            _caseModel.InstrumentName = null;

            //act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.CompleteCases(_caseModel));
            Assert.AreEqual("InstrumentName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_instrumentName_When_I_Call_CompleteCases_Then_An_ArgumentException_Is_Thrown()
        {   
            //arrange
            _caseModel.InstrumentName = string.Empty;

            var exception = Assert.Throws<ArgumentException>(() => _sut.CompleteCases(_caseModel));
            Assert.AreEqual("A value for the argument 'InstrumentName' must be supplied", exception.Message);
        }

        [TestCase(-1)]
        [TestCase(0)]
        public void Given_An_Invalid_NumberOfCasesToComplete_When_I_Call_CompleteCases_Then_An_ArgumentException_Is_Thrown(int numberOfCases)
        {
            //arrange
            _caseModel.NumberOfCases = numberOfCases;

            var exception = Assert.Throws<ArgumentException>(() => _sut.CompleteCases(_caseModel));
            Assert.AreEqual("NumberOfCases", exception.Message);
        }

        [Test]
        public void Given_Survey_Does_Not_Exist_When_I_Call_CompleteCases_No_Cases_Are_Processed()
        {
            //arrange
            _blaiseApiMock.Setup(b => b.Survey.Exists).Returns(false);

            //act
            _sut.CompleteCases(_caseModel);

            //assert
            _completeCaseServiceMock.VerifyNoOtherCalls();
        }

        [Test]
        public void Given_All_Cases_Are_Already_Complete_When_I_Call_CompleteCases_No_Cases_Are_Processed()
        {
            //arrange
            _blaiseApiMock.Setup(b => b.Survey.Exists).Returns(true);
            _dataSetMock.SetupSequence(d => d.EndOfSet)
                .Returns(false)
                .Returns(true);

            _blaiseApiMock.Setup(d => d.Cases).Returns(_dataSetMock.Object);
            _blaiseApiMock.Setup(s => s.Case.WithDataRecord(It.IsAny<IDataRecord>()).Completed).Returns(true);

            //act
            _sut.CompleteCases(_caseModel);

            //assert
            _completeCaseServiceMock.Verify(v => v.CompleteCase(It.IsAny<IDataRecord>(), _caseModel), Times.Never);
        }

        [TestCase(1, 1, 1)]
        [TestCase(10, 10, 10)]
        [TestCase(5, 10, 5)]
        [TestCase(10, 5, 5)]
        public void Given_X_Cases_To_Complete_And_Y_Cases_Available_When_I_Call_CompleteCases_Then_The_Expected_Cases_Are_Completed(int casesToComplete, int casesAvailable, int expectedCases)
        {
            //arrange
            _caseModel.NumberOfCases = casesToComplete;

            _blaiseApiMock.Setup(b => b.Survey.Exists).Returns(true);

            var sequence = _dataSetMock.SetupSequence(c => c.EndOfSet);
            for (int i = 0; i < casesAvailable; i++)
            {
                sequence.Returns(false);
            }
            sequence.Returns(true);

            _dataSetMock.Setup(s => s.ActiveRecord).Returns(It.IsAny<IDataRecord>());

            _blaiseApiMock.Setup(d => d.Cases).Returns(_dataSetMock.Object);
            _blaiseApiMock.Setup(s => s.Case.WithDataRecord(It.IsAny<IDataRecord>()).Completed).Returns(false);

            //act
            _sut.CompleteCases(_caseModel);

            //assert
            _completeCaseServiceMock.Verify(v => v.CompleteCase(It.IsAny<IDataRecord>(), _caseModel), Times.Exactly(expectedCases));
        }
    }
}
