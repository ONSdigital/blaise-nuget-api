namespace Blaise.Nuget.Api.Core.Interfaces.Providers
{
    using Blaise.Nuget.Api.Contracts.Models;

    public interface IBlaiseConfigurationProvider
    {
        int ConnectionExpiresInMinutes { get; }

        string DatabaseConnectionString { get; }

        ConnectionModel GetConnectionModel();
    }
}
