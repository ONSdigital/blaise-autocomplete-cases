using BlaiseAutoCompleteCases.Interfaces.Providers;
using System.Configuration;

namespace BlaiseAutoCompleteCases.Providers
{
    public class ConfigurationProvider : IConfigurationProvider
    {
        public string ProjectId => ConfigurationManager.AppSettings["ProjectId"];

        public string SubscriptionId => ConfigurationManager.AppSettings["SubscriptionId"];

        public string TopicId => ConfigurationManager.AppSettings["TopicId"];      
    }
}
