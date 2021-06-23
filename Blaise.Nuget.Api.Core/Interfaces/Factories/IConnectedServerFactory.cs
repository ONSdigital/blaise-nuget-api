using Blaise.Nuget.Api.Contracts.Models;
using StatNeth.Blaise.API.ServerManager;

namespace Blaise.Nuget.Api.Core.Interfaces.Factories
{
    public interface IConnectedServerFactory {
        IConnectedServer GetConnection(ConnectionModel connectionModel);

        IConnectedServer GetIsolatedConnection(ConnectionModel connectionModel);
    }
}
