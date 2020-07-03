using System.Configuration;
using BlaiseCaseAutoComplete.Interfaces.Providers;

namespace BlaiseCaseAutoComplete.Providers
{
    public class ConfigurationProvider : IConfigurationProvider
    {
        public string ProjectId => ConfigurationManager.AppSettings["ProjectId"];

        public string SubscriptionId => ConfigurationManager.AppSettings["SubscriptionId"];
    }
}
