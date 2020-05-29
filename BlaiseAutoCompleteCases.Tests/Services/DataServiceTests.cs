using Blaise.Nuget.Api.Contracts.Interfaces;
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
    public class DataServiceTests
    {
        private DataService _sut;
        private Mock<ILog> _loggingMock;
        private Mock<IBlaiseApi> _blaiseApiMock;
        private Mock<IDataSet> _dataSetMock;
        private readonly string _instrumentName;
        private readonly string _serverParkName;

        public DataServiceTests()
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

            _sut = new DataService(_loggingMock.Object, _blaiseApiMock.Object);

        }

        [Test]
        public void Given_A_Null_InstrumentName_When_I_Call_CompleteCaseService_Then_A_ArgumentNullException_Is_Thrown()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetDataAndUpdate(It.IsAny<IDataRecord>(), null, _serverParkName));
            Assert.AreEqual("instrumentName", exception.ParamName);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_CompleteCaseService_Then_A_ArgumentNullException_Is_Thrown()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetDataAndUpdate(It.IsAny<IDataRecord>(), _instrumentName, null));
            Assert.AreEqual("serverParkName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_InstrumentName_When_I_Call_CompleteCaseService_Then_A_ArgumentNullException_Is_Thrown()
        {
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetDataAndUpdate(It.IsAny<IDataRecord>(), string.Empty, _serverParkName));
            Assert.AreEqual("A value for the argument 'instrumentName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_CompleteCaseService_Then_A_ArgumentNullException_Is_Thrown()
        {
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetDataAndUpdate(It.IsAny<IDataRecord>(), _instrumentName, string.Empty));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        //[Test]
        //public void Given_A_Case_When_I_Call_CompleteCaseService_Then_The_Case_Is_Completed()
        //{
        //    //_dataSetMock = new Mock<IDataSet>();

        //    //_dataSetMock.Setup(s => s.ActiveRecord).Returns(It.IsAny<IDataRecord>());
        //    //_blaiseApiMock.Setup(d => d.GetDataSet(_instrumentName, _serverParkName)).Returns(_dataSetMock.Object);
        //    var dictionary = new Dictionary<string, string>
        //    {
        //        {"QID.HHold", "1"},
        //    };

        //    var dataSetMock = new Mock<IDataSet>();
        //    _blaiseApiMock.Setup(b => b.GetDataSet(It.IsAny<string>(), It.IsAny<string>())).Returns(dataSetMock.Object);

        //    _sut.GetDataAndUpdate(dataSetMock.Object.ActiveRecord, _instrumentName, _serverParkName);

        //    //assert
        //    _blaiseApiMock.Verify(v => v.UpdateDataRecord(dataSetMock.Object.ActiveRecord, dictionary, _instrumentName, _serverParkName), Times.Once);
        //}
    }
}
