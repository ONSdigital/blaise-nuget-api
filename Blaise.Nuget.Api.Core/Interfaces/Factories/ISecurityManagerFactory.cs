using Blaise.Nuget.Api.Contracts.Models;
using StatNeth.Blaise.API.Security;

namespace Blaise.Nuget.Api.Core.Interfaces.Factories
{
    public interface ISecurityManagerFactory
    {
        ISecurityServer GetConnection(ConnectionModel connectionModel);
    }
}
