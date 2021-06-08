
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Admin;
using StatNeth.Blaise.API.DataLink;

namespace Blaise.Nuget.Api.Core.Interfaces.Providers
{
    public interface ILocalDataLinkProvider : IResetConnections, IGetOpenConnections
    {
        IDataLink4 GetDataLink(ConnectionModel connectionModel, string databaseFile);
    }
}
