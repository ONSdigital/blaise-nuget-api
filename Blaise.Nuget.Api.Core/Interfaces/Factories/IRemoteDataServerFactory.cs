using Blaise.Nuget.Api.Contracts.Models;
using StatNeth.Blaise.API.DataLink;

namespace Blaise.Nuget.Api.Core.Interfaces.Factories
{
    public interface IRemoteDataServerFactory
    {
        IRemoteDataServer GetConnection(ConnectionModel connectionModel);
    }
}
