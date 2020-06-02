using System.Collections.Generic;
using BlaiseCaseAutoComplete.Interfaces.PersonData;
using BlaiseCaseAutoComplete.PersonData;
using Moq;
using NUnit.Framework;

namespace BlaiseCaseAutoComplete.Tests.PersonData
{
    public class PersonOutComeTests
    {
        private readonly Mock<IPersonOutcome> _personOutComeMock;
        private PersonOutcome _sut;
        public PersonOutComeTests()
        {
            _personOutComeMock = new Mock<IPersonOutcome>();
        }

        [SetUp]
        public void SetUpTests()
        {
            _sut = new PersonOutcome();
        }

        [Test]
        public void Given_I_Call_GetPersonOutComeData_Then_The_Correct_Call_Is_Made()
        {
            //Arrange & Assert
            var dictionaryMock = new Dictionary<string, string>
            {
                {"QID.HHold", "1"},
                {"QHAdmin.HOut", "110"},
                {"QHAdmin.IntNum", "1001"},
                {"Mode", "1"},
                {"Processed", "2"},
                {"QDataBag.PostCode", "XX999XX"},
                {"QHousehold.QHHold.Person[1].Sex", "1"},
                {"QHousehold.QHHold.Person[1].tmpDoB", "1/1/1980"},
                {"QHousehold.QHHold.Person[1].DVAge", "40"}
            };
            var dictionary = _sut.GetPersonOutcomeData_Good();

            //Act
            Assert.AreEqual(dictionary, dictionaryMock);
        }
    }
}
