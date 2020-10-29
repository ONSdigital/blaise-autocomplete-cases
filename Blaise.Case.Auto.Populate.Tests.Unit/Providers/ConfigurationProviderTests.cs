using Blaise.Case.Auto.Populate.Providers;
using NUnit.Framework;

namespace Blaise.Case.Auto.Populate.Tests.Unit.Providers
{
    public class ConfigurationProviderTests
    {
        /// <summary>
        /// Please ensure the app.config in the test project has values that relate to the tests
        /// </summary>

        [Test]
        public void Given_I_Call_ProjectId_I_Get_The_Correct_Value_Back()
        {
            //arrange
            var configurationProvider = new LocalConfigurationProvider();

            //act
            var result = configurationProvider.ProjectId;

            //assert
            Assert.AreEqual("ProjectIdTest", result);
        }

        [Test]
        public void Given_I_Call_SubscriptionId_I_Get_The_Correct_Value_Back()
        {
            //arrange
            var configurationProvider = new LocalConfigurationProvider();

            //act
            var result = configurationProvider.SubscriptionId;

            //assert
            Assert.AreEqual("SubscriptionIdTest", result);
        }

        [Test]
        public void Given_I_Call_VmName_I_Get_The_Correct_Value_Back()
        {
            //arrange
            var configurationProvider = new LocalConfigurationProvider();

            //act
            var result = configurationProvider.VmName;

            //assert
            Assert.AreEqual("VmNameTest", result);
        }
    }
}
