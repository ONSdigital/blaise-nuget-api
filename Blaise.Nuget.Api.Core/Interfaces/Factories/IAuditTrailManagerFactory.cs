namespace Blaise.Nuget.Api.Core.Interfaces.Factories
{
    using Blaise.Nuget.Api.Contracts.Models;
    using StatNeth.Blaise.API.AuditTrail;

    public interface IAuditTrailManagerFactory
    {
        IRemoteAuditTrailServer GetRemoteAuditTrailServer(ConnectionModel connectionModel);
    }
}
