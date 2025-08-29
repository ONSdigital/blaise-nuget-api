using Blaise.Nuget.Api.Contracts.Models;

namespace Blaise.Nuget.Api.Core.Interfaces.Providers
{
    public interface IBlaiseConfigurationProvider
    {
        int ConnectionExpiresInMinutes { get; }

        string DatabaseConnectionString { get; }

        ConnectionModel GetConnectionModel();
    }
}
