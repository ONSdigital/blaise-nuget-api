using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Admin;
using StatNeth.Blaise.API.Security;

namespace Blaise.Nuget.Api.Core.Interfaces.Factories
{
    public interface ISecurityManagerFactory : IResetConnections, IGetOpenConnections
    {
        ISecurityServer GetConnection(ConnectionModel connectionModel);
    }
}