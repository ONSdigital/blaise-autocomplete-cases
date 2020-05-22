using BlaiseAutoCompleteCases.Interfaces.Mappers;
using BlaiseAutoCompleteCases.Interfaces.Services;
using log4net;
using Moq;
using NUnit.Framework;

namespace BlaiseAutoCompleteCases.Tests.Services
{
    public class FindCasesToCompleteServiceTests
    {
        private Mock<ILog> _loggingMock;
        private Mock<ICompletionModelMapper> _mapperMock;
        private Mock<IFindCasesToCompleteService> _findCasesToCompleteService;

        [SetUp]
        public void SetUpTests()
        {
            _loggingMock = new Mock<ILog>();
            _mapperMock = new Mock<ICompletionModelMapper>();
            _findCasesToCompleteService = new Mock<IFindCasesToCompleteService>();
            _findCasesToCompleteService.Setup(p => p.FindCasesToComplete(It.IsAny<string>(), 0));
        }

        [Test]
        public void Given_A_Null_SurveyName_When_I_Call_FindCasesToCompleteService_Then_A_NullArgumentException_Is_Thrown()
        {
            //arrange
            _findCasesToCompleteService.Setup(p => p.FindCasesToComplete(It.IsAny<string>(), 0));

            //act

            //assert
        }

        [Test]
        public void Given_An_Empty_SurveyName_When_I_Call_FindCasesToCompleteService_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange

            //act

            //assert
        }

        [TestCase(-1)]
        [TestCase(0)]
        public void Given_An_Invalid_NumberOfCasesToComplete_When_I_Call_FindCasesToCompleteService_Then_An_ArgumentException_Is_Thrown(int numberOfCasesToComplete)
        {
            //arrange

            //act

            //assert
        }

        [Test]
        public void Given_No_Instruments_Available_When_I_Call_FindCasesToCompleteService_No_Cases_Are_Processed()
        {
            //arrange

            //act

            //assert
        }

        [Test]
        public void Given_No_Instruments_Available_When_I_Call_FindCasesToCompleteService_An_Appropriate_Message_Is_Logged()
        {
            //arrange

            //act

            //assert
        }

        [Test]
        public void Given_All_Cases_Are_Already_Complete_When_I_Call_FindCasesToCompleteService_No_Cases_Are_Processed()
        {
            //arrange

            //act

            //assert
        }

        [Test]
        public void Given_All_Cases_Are_Already_Complete_When_I_Call_FindCasesToCompleteService_An_Appropriate_Message_Is_Logged()
        {
            //arrange

            //act

            //assert
        }

        [TestCase(1, 1)]
        [TestCase(10, 10)]
        [TestCase(10, 5)]
        [TestCase(5, 10)]
        public void Given_X_Cases_To_Complete_And_Y_Cases_Available_When_I_Call_FindCasesToCompleteService_Then_The_Appropriate_Cases_Are_Completed(int x, int y)
        {
            //arrange

            //act

            //assert
        }

    }
}
