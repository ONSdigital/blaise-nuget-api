namespace Blaise.Nuget.Api.Core.Interfaces.Mappers
{
    using System;
    using System.Collections.Generic;
    using Blaise.Nuget.Api.Contracts.Models;
    using StatNeth.Blaise.API.AuditTrail;

    public interface IAuditTrailDataMapper
    {
        AuditTrailDataModel MapAuditTrailDataModel(string keyValue, Guid sessionId, IEventInfo eventInfo);

        string MapAuditTrailCsvContent(List<AuditTrailDataModel> listOfEvents);
    }
}
