using Blaise.Nuget.Api.Contracts.Models;
using StatNeth.Blaise.API.Cati.Runtime;

namespace Blaise.Nuget.Api.Core.Interfaces.Providers
{
    public interface IRemoteCatiManagementServerProvider 
    {
        IRemoteCatiManagementServer GetCatiManagementForServerPark(ConnectionModel connectionModel, string serverParkName);
    }
}