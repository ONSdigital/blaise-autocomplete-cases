using System.Configuration;
using Blaise.Case.Auto.Populate.Interfaces.Providers;

namespace Blaise.Case.Auto.Populate.Providers
{
    public class LocalConfigurationProvider : IConfigurationProvider
    {
        public string ProjectId => ConfigurationManager.AppSettings["ProjectId"];

        public string SubscriptionId => ConfigurationManager.AppSettings["SubscriptionId"];

        public string VmName => ConfigurationManager.AppSettings["VmName"];
    }
}
