using System;
using System.Collections.Generic;
using BlaiseAutoCompleteCases.Interfaces.PersonData;
using BlaiseAutoCompleteCases.PersonData;
using Moq;
using NUnit.Framework;

namespace BlaiseAutoCompleteCases.Tests.PersonData
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
                {"QHAdmin.HOut", "310"},
                {"QHAdmin.IntNum", "1001"},
                {"Mode", "1"},
                {"QDataBag.PostCode", "XX999XX"},
                {"QHousehold.QHHold.Person[1].Sex", "1"},
                {"QHousehold.QHHold.Person[1].tmpDoB", "1/1/1980"},
                {"QHousehold.QHHold.Person[1].DVAge", "40"}
            };
            var dictionary = _sut.getPersonOutcomeData();

            //Act
            Assert.AreEqual(dictionary, dictionaryMock);
        }
    }
}
