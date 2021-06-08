using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Admin;
using StatNeth.Blaise.API.Cati.Runtime;

namespace Blaise.Nuget.Api.Core.Interfaces.Factories
{
    public interface ICatiManagementServerFactory : IResetConnections, IGetOpenConnections
    {
        IRemoteCatiManagementServer GetConnection(ConnectionModel connectionModel);
    }
}