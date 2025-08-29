using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Mappers;
using StatNeth.Blaise.API.AuditTrail;

namespace Blaise.Nuget.Api.Core.Mappers
{
    public class AuditTrailDataMapper : IAuditTrailDataMapper
    {
        /// <inheritdoc/>
        public AuditTrailDataModel MapAuditTrailDataModel(string keyValue, Guid sessionId, IEventInfo eventInfo)
        {
            return new AuditTrailDataModel
            {
                KeyValue = keyValue,
                SessionId = sessionId,
                TimeStamp = eventInfo.TimeStamp,
                Content = eventInfo.ToString()
            };
        }

        /// <inheritdoc/>
        public string MapAuditTrailCsvContent(List<AuditTrailDataModel> listOfEvents)
        {
            var csvContent = new StringBuilder();
            csvContent.AppendLine("KeyValue,SessionId,Timestamp,Content");

            foreach (var eventFromAudit in listOfEvents.OrderBy(o => o.KeyValue))
            {
                csvContent.AppendLine($"{eventFromAudit.KeyValue}, {eventFromAudit.SessionId}, {eventFromAudit.TimeStamp:dd/MM/yyyy HH:mm:ss}, {eventFromAudit.Content}");
            }

            return csvContent.ToString();
        }
    }
}
