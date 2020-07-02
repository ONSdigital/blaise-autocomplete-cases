using System;
using System.Collections.Generic;
using Blaise.Nuget.Api.Contracts.Enums;
using Blaise.Nuget.Api.Contracts.Interfaces;
using BlaiseCaseAutoComplete.Models;
using BlaiseCaseAutoComplete.Services;
using log4net;
using Moq;
using NUnit.Framework;
using StatNeth.Blaise.API.DataRecord;
using StatNeth.Blaise.API.ServerManager;

namespace BlaiseCaseAutoComplete.Tests.Services
{
    public class CompleteCaseServiceTests
    {
        private CompleteCaseService _sut;
        private Mock<ILog> _loggingMock;
        private Mock<IFluentBlaiseApi> _blaiseApiMock;
        private readonly string _instrumentName;
        private readonly string _serverParkName;
        private Mock<IDataRecord> _dataRecord;
        private AutoCompleteCaseModel _caseModel;

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
            _caseModel = new AutoCompleteCaseModel
            {
                InstrumentName = _instrumentName,
                ServerPark = _serverParkName,
                PrimaryKey = "Key1",
                NumberOfCases = 10,
                Payload = new Dictionary<string, string>
                {
                    {"Key1", "Value1"}
                }
            };

            _dataRecord = new Mock<IDataRecord>();
            _dataRecord.Setup(d => d.Keys[0].KeyValue).Returns("");

            _loggingMock = new Mock<ILog>();

            _blaiseApiMock = new Mock<IFluentBlaiseApi>();
            _blaiseApiMock.Setup(b => b.WithInstrument(It.IsAny<string>())).Returns(_blaiseApiMock.Object);
            _blaiseApiMock.Setup(b => b.WithServerPark(It.IsAny<string>())).Returns(_blaiseApiMock.Object);
            
            _blaiseApiMock.Setup(b => b
                .Case
                .WithPrimaryKey(It.IsAny<string>())
                .Get()).Returns(_dataRecord.Object);

            _blaiseApiMock.Setup(b => b
                .Case
                .WithDataRecord(It.IsAny<IDataRecord>())
                .WithData(It.IsAny<Dictionary<string, string>>())
                .Update());

            _blaiseApiMock.Setup(b => b
                .Case
                .WithDataRecord(It.IsAny<IDataRecord>())
                .WithStatus(It.IsAny<StatusType>())
                .Update());

            _sut = new CompleteCaseService(_loggingMock.Object, _blaiseApiMock.Object);
        }

        [Test]
        public void Given_A_Null_PrimaryKey_When_I_Call_CompleteCaseService_With_No_DataRecord_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            _caseModel.PrimaryKey = null;

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.CompleteCase( _caseModel));
            Assert.AreEqual("PrimaryKey", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_PrimaryKey_When_I_Call_CompleteCaseService_With_No_DataRecord_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            _caseModel.PrimaryKey = string.Empty;

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.CompleteCase(_caseModel));
            Assert.AreEqual("A value for the argument 'PrimaryKey' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_InstrumentName_When_I_Call_CompleteCaseService_With_No_DataRecord_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            _caseModel.InstrumentName = null;

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.CompleteCase(_caseModel));
            Assert.AreEqual("InstrumentName", exception.ParamName);
        }


        [Test]
        public void Given_An_Empty_InstrumentName_When_I_Call_CompleteCaseService_With_No_DataRecord_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            _caseModel.InstrumentName = string.Empty;

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.CompleteCase(_caseModel));
            Assert.AreEqual("A value for the argument 'InstrumentName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_CompleteCaseService_With_No_DataRecord_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            _caseModel.ServerPark = null;

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.CompleteCase(_caseModel));
            Assert.AreEqual("ServerPark", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_CompleteCaseService_With_No_DataRecord_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            _caseModel.ServerPark = string.Empty;

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.CompleteCase( _caseModel));
            Assert.AreEqual("A value for the argument 'ServerPark' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Case_Exists_When_I_Call_CompleteCase_With_No_DataRecord_Then_A_Call_To_MarkCaseAsComplete_Is_Made()
        {
            //arrange
            _blaiseApiMock.Setup(b => b
                .Case
                .WithPrimaryKey(It.IsAny<string>())
                .Exists).Returns(true);

            //act
            _sut.CompleteCase(_caseModel);

            //assert
            _blaiseApiMock.Verify(v => v
                .Case
                .WithDataRecord(_dataRecord.Object)
                .WithStatus(StatusType.Completed)
                .Update(), Times.Once);
        }

        [Test]
        public void Given_A_Case_Does_Not_Exist_When_I_Call_CompleteCase_With_No_DataRecord_Then_A_Call_To_MarkCaseAsComplete_Is_Not_Made()
        {
            //arrange
            _blaiseApiMock.Setup(b => b
                .Case
                .WithPrimaryKey(It.IsAny<string>())
                .Exists).Returns(false);

            //act
            _sut.CompleteCase(_caseModel);

            //assert
            _blaiseApiMock.Verify(v => v
                .Case
                .WithDataRecord(It.IsAny<IDataRecord>())
                .WithStatus(It.IsAny<StatusType>())
                .Update(), Times.Never);
        }

        [Test]
        public void Given_A_Case_When_I_Call_CompleteCase_With_No_DataRecord_Then_A_Call_To_UpdateDataRecord_Is_Made()
        {
            //arrange
            _blaiseApiMock.Setup(b => b.Case.WithPrimaryKey(_caseModel.PrimaryKey).Get()).Returns(_dataRecord.Object);

            //act
            _sut.CompleteCase(_dataRecord.Object, _caseModel);

            //assert
            _blaiseApiMock.Verify(v => v.Case.WithDataRecord(_dataRecord.Object).WithData(_caseModel.Payload).Update(), Times.Once);
        }


        [Test]
        public void Given_A_Null_PrimaryKey_When_I_Call_CompleteCaseService_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            _caseModel.PrimaryKey = null;

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.CompleteCase(It.IsAny<IDataRecord>(), _caseModel));
            Assert.AreEqual("PrimaryKey", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_PrimaryKey_When_I_Call_CompleteCaseService_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            _caseModel.PrimaryKey = string.Empty;

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.CompleteCase(It.IsAny<IDataRecord>(), _caseModel));
            Assert.AreEqual("A value for the argument 'PrimaryKey' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_InstrumentName_When_I_Call_CompleteCaseService_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            _caseModel.InstrumentName = null;

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.CompleteCase(_dataRecord.Object, _caseModel));
            Assert.AreEqual("InstrumentName", exception.ParamName);
        }


        [Test]
        public void Given_An_Empty_InstrumentName_When_I_Call_CompleteCaseService_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            _caseModel.InstrumentName = string.Empty;

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.CompleteCase(It.IsAny<IDataRecord>(), _caseModel));
            Assert.AreEqual("A value for the argument 'InstrumentName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_CompleteCaseService_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            _caseModel.ServerPark = null;

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.CompleteCase(It.IsAny<IDataRecord>(), _caseModel));
            Assert.AreEqual("ServerPark", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_CompleteCaseService_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            _caseModel.ServerPark = string.Empty;

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.CompleteCase(It.IsAny<IDataRecord>(), _caseModel));
            Assert.AreEqual("A value for the argument 'ServerPark' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Case_When_I_Call_CompleteCase_Then_A_Call_To_MarkCaseAsComplete_Is_Made()
        {
            //act
            _sut.CompleteCase(_dataRecord.Object, _caseModel);

            //assert
            _blaiseApiMock.Verify(v => v.Case
                .WithDataRecord(_dataRecord.Object)
                .WithStatus(StatusType.Completed)
                .Update()
                , Times.Once);
        }

        [Test]
        public void Given_A_Case_When_I_Call_CompleteCase_Then_A_Call_To_UpdateDataRecord_Is_Made()
        {
            //act
            _sut.CompleteCase(_dataRecord.Object, _caseModel);

            //assert
            _blaiseApiMock.Verify(v => v.Case
                    .WithDataRecord(_dataRecord.Object)
                    .WithData(_caseModel.Payload)
                    .Update()
                , Times.Once);
        }
    }
}
