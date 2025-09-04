namespace Blaise.Nuget.Api.Core.Interfaces.Factories
{
    using Blaise.Nuget.Api.Contracts.Models;
    using StatNeth.Blaise.API.Security;

    public interface ISecurityManagerFactory
    {
        ISecurityServer GetConnection(ConnectionModel connectionModel);
    }
}
