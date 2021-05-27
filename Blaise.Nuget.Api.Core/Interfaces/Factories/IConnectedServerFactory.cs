using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Admin;
using StatNeth.Blaise.API.ServerManager;

namespace Blaise.Nuget.Api.Core.Interfaces.Factories
{
    public interface IConnectedServerFactory : IResetConnections
    {
        IConnectedServer GetConnection(ConnectionModel connectionModel);

        IConnectedServer GetIsolatedConnection(ConnectionModel connectionModel);
    }
}
