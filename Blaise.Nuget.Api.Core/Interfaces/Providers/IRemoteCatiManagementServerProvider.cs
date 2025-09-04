namespace Blaise.Nuget.Api.Core.Interfaces.Providers
{
    using Blaise.Nuget.Api.Contracts.Models;
    using StatNeth.Blaise.API.Cati.Runtime;

    public interface IRemoteCatiManagementServerProvider
    {
        IRemoteCatiManagementServer GetCatiManagementForServerPark(ConnectionModel connectionModel, string serverParkName);
    }
}
