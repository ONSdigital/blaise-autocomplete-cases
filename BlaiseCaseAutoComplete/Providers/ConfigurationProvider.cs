using System;
using System.Configuration;
using BlaiseCaseAutoComplete.Interfaces.Providers;

namespace BlaiseCaseAutoComplete.Providers
{
    public class ConfigurationProvider : IConfigurationProvider
    {
        public string ProjectId => Environment.GetEnvironmentVariable("ENV_PROJECT_ID", EnvironmentVariableTarget.Machine);

        public string SubscriptionId => Environment.GetEnvironmentVariable("ENV_BCA_SUB_SUBS", EnvironmentVariableTarget.Machine) ;

        public string VmName => Environment.MachineName;
    }
}
