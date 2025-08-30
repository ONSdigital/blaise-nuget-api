namespace Blaise.Nuget.Api.Core.Interfaces.Factories
{
    using Blaise.Nuget.Api.Contracts.Models;
    using StatNeth.Blaise.API.DataLink;

    public interface IRemoteDataServerFactory
    {
        IRemoteDataServer GetConnection(ConnectionModel connectionModel);
    }
}
