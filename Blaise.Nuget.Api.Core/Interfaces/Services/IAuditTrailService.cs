using Blaise.Nuget.Api.Contracts.Models;

namespace Blaise.Nuget.Api.Core.Interfaces.Services
{
    public interface IAuditTrailService
    {
        byte[] GetAuditTrailData(ConnectionModel connectionModel, string questionnaireName, string serverParkName);
    }
}