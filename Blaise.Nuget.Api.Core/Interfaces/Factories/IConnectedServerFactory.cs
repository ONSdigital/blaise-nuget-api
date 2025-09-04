namespace Blaise.Nuget.Api.Core.Interfaces.Factories
{
    using Blaise.Nuget.Api.Contracts.Models;
    using StatNeth.Blaise.API.ServerManager;

    public interface IConnectedServerFactory
    {
        IConnectedServer GetConnection(ConnectionModel connectionModel);

        IConnectedServer GetIsolatedConnection(ConnectionModel connectionModel);
    }
}
