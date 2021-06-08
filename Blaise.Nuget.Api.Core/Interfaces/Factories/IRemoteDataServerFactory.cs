using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Admin;
using StatNeth.Blaise.API.DataLink;

namespace Blaise.Nuget.Api.Core.Interfaces.Factories
{
    public interface IRemoteDataServerFactory : IResetConnections, IGetOpenConnections
    {
        IRemoteDataServer GetConnection(ConnectionModel connectionModel);
    }
}
