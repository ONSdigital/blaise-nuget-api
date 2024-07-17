using Blaise.Nuget.Api.Contracts.Models;
using StatNeth.Blaise.API.DataLink;

namespace Blaise.Nuget.Api.Core.Interfaces.Providers
{
    public interface ILocalDataLinkProvider 
    {
        IDataLink6 GetDataLink(ConnectionModel connectionModel, string databaseFile);
    }
}
