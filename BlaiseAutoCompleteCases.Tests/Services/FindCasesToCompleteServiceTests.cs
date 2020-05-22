using NUnit.Framework;

namespace BlaiseAutoCompleteCases.Tests.Services
{
    public class FindCasesToCompleteServiceTests
    {

        [Test]
        public void Given_A_Null_InstrumentName_When_I_Call_FindCasesToCompleteService_Then_A_NullArgumentException_Is_Thrown()
        {
            //arrange

            //act

            //assert
        }

        [Test]
        public void Given_An_Empty_InstrumentName_When_I_Call_FindCasesToCompleteService_Then_An_ArgumentException_Is_Thrown()
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
