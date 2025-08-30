namespace Blaise.Nuget.Api.Core.Interfaces.Mappers
{
    using Blaise.Nuget.Api.Contracts.Models;
    using StatNeth.Blaise.API.AuditTrail;
    using System;
    using System.Collections.Generic;

    public interface IAuditTrailDataMapper
    {
        AuditTrailDataModel MapAuditTrailDataModel(string keyValue, Guid sessionId, IEventInfo eventInfo);

        string MapAuditTrailCsvContent(List<AuditTrailDataModel> listOfEvents);
    }
}
