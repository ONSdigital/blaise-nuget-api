namespace Blaise.Nuget.Api.Contracts.Interfaces
{
    using System.Collections.Generic;
    using Blaise.Nuget.Api.Contracts.Models;

    public interface IBlaiseAuditTrailApi
    {
        IEnumerable<AuditTrailDataModel> GetAuditTrail(string serverPark, string questionnaireName);
    }
}
