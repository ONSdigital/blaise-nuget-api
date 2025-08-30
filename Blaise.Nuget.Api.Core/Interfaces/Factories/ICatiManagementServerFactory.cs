namespace Blaise.Nuget.Api.Core.Interfaces.Factories
{
    using Blaise.Nuget.Api.Contracts.Models;
    using StatNeth.Blaise.API.Cati.Runtime;

    public interface ICatiManagementServerFactory
    {
        IRemoteCatiManagementServer GetConnection(ConnectionModel connectionModel);
    }
}
