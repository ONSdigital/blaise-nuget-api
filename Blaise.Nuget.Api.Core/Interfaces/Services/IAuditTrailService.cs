using System.Collections.Generic;
using Blaise.Nuget.Api.Contracts.Models;

namespace Blaise.Nuget.Api.Core.Interfaces.Services
{
    public interface IAuditTrailService
    {
        List<AuditTrailData> GetAuditTrailData(ConnectionModel connectionModel, string questionnaireName, string serverParkName);
        string GenerateCsvContent(List<AuditTrailData> listOfEvents);
    }
}