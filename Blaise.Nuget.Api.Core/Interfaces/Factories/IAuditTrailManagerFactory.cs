using Blaise.Nuget.Api.Contracts.Models;
using StatNeth.Blaise.API.AuditTrail;

namespace Blaise.Nuget.Api.Core.Interfaces.Factories
{
    public interface IAuditTrailManagerFactory
    {
        IRemoteAuditTrailServer GetRemoteAuditTrailServer(ConnectionModel connectionModel);
    }
}
