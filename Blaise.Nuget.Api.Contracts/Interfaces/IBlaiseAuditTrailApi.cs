using System.Collections.Generic;
using Blaise.Nuget.Api.Contracts.Models;

namespace Blaise.Nuget.Api.Contracts.Interfaces
{
    public interface IBlaiseAuditTrailApi
    {
        IEnumerable<AuditTrailDataModel> GetAuditTrail(string serverPark, string questionnaireName);
    }
}
