namespace BlaiseCaseAutoComplete.Interfaces.Providers
{
    public interface IConfigurationProvider
    {
        string ProjectId { get; }

        string SubscriptionId { get; }
    }
}
