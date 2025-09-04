namespace Blaise.Nuget.Api.Core.Interfaces.Services
{
    using System.Collections.Generic;
    using Blaise.Nuget.Api.Contracts.Models;

    public interface IAuditTrailService
    {
        List<AuditTrailDataModel> GetAuditTrailData(ConnectionModel connectionModel, string questionnaireName, string serverParkName);

        string CreateAuditTrailCsvContent(ConnectionModel connectionModel, string questionnaireName, string serverParkName);
    }
}
