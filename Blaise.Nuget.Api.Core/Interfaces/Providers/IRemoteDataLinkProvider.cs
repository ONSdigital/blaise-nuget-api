using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Admin;
using StatNeth.Blaise.API.DataLink;

namespace Blaise.Nuget.Api.Core.Interfaces.Providers
{
    public interface IRemoteDataLinkProvider : IResetConnections, IGetOpenConnections
    {
        IDataLink4 GetDataLink(ConnectionModel connectionModel, string instrumentName, string serverParkName);
    }
}
