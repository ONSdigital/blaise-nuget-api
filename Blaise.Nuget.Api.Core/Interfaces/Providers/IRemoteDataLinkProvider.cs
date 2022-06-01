using Blaise.Nuget.Api.Contracts.Models;
using StatNeth.Blaise.API.DataLink;

namespace Blaise.Nuget.Api.Core.Interfaces.Providers
{
    public interface IRemoteDataLinkProvider {
        IDataLink4 GetDataLink(ConnectionModel connectionModel, string questionnaireName, string serverParkName);
    }
}
