namespace Blaise.Nuget.Api.Core.Interfaces.Services
{
    using Blaise.Nuget.Api.Contracts.Models;
    using System.Collections.Generic;

    public interface IAuditTrailService
    {
        List<AuditTrailDataModel> GetAuditTrailData(ConnectionModel connectionModel, string questionnaireName, string serverParkName);

        string CreateAuditTrailCsvContent(ConnectionModel connectionModel, string questionnaireName, string serverParkName);
    }
}
