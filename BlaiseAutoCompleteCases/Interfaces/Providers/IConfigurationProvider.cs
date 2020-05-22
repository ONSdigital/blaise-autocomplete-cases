namespace BlaiseAutoCompleteCases.Interfaces.Providers
{
    public interface IConfigurationProvider
    {
        string ProjectId { get; }

        string SubscriptionId { get; }

        string TopicId { get; }
    }
}
