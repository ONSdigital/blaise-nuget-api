namespace Blaise.Nuget.Api.Contracts.Interfaces
{
    using Blaise.Nuget.Api.Contracts.Models;
    using System.Collections.Generic;

    public interface IBlaiseAuditTrailApi
    {
        IEnumerable<AuditTrailDataModel> GetAuditTrail(string serverPark, string questionnaireName);
    }
}
