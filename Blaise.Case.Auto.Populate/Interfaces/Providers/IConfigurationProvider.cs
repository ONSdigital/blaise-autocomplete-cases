namespace Blaise.Case.Auto.Populate.Interfaces.Providers
{
    public interface IConfigurationProvider
    {
        string ProjectId { get; }

        string SubscriptionId { get; }

        string VmName { get; }
    }
}
