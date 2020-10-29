using System.Configuration;
using BlaiseCaseAutoComplete.Interfaces.Providers;

namespace BlaiseCaseAutoComplete.Providers
{
    public class LocalConfigurationProvider : IConfigurationProvider
    {
        public string ProjectId => ConfigurationManager.AppSettings["ProjectId"];

        public string SubscriptionId => ConfigurationManager.AppSettings["SubscriptionId"];

        public string VmName => ConfigurationManager.AppSettings["VmName"];
    }
}
