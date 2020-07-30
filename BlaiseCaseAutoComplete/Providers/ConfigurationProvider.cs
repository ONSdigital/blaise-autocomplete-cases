using System.Configuration;
using BlaiseCaseAutoComplete.Interfaces.Providers;

namespace BlaiseCaseAutoComplete.Providers
{
    public class ConfigurationProvider : IConfigurationProvider
    {
        public string ProjectId => Environment.GetEnvironmentVariable("ENV_PROJECT_ID", EnvironmentVariableTarget.Machine) ?? ConfigurationManager.AppSettings["ProjectId"];

        public string SubscriptionId => ConfigurationManager.AppSettings["SubscriptionId"];
    }
}
