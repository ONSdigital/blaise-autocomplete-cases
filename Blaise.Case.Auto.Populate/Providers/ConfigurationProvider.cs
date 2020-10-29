using System;
using Blaise.Case.Auto.Populate.Interfaces.Providers;

namespace Blaise.Case.Auto.Populate.Providers
{
    public class ConfigurationProvider : IConfigurationProvider
    {
        public string ProjectId => Environment.GetEnvironmentVariable("ENV_PROJECT_ID", EnvironmentVariableTarget.Machine);

        public string SubscriptionId => Environment.GetEnvironmentVariable("ENV_BCA_SUB_SUBS", EnvironmentVariableTarget.Machine) ;

        public string VmName => Environment.MachineName;
    }
}
