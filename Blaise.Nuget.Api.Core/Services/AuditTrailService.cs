using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Factories;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using StatNeth.Blaise.API.AuditTrail;

namespace Blaise.Nuget.Api.Core.Services
{
    public class AuditTrailService : IAuditTrailService
    {
        private readonly IQuestionnaireService _questionnaireService;
        private readonly IAuditTrailManagerFactory _auditTrailManagerFactory;

        public AuditTrailService(
            IQuestionnaireService questionnaireService, 
            IAuditTrailManagerFactory auditTrailManagerFactory)
        {
            _questionnaireService = questionnaireService;
            _auditTrailManagerFactory = auditTrailManagerFactory;
        }

        public List<AuditTrailDataModel> GetAuditTrailData(ConnectionModel connectionModel, string questionnaireName, string serverParkName)
        {
            var instrumentId = _questionnaireService.GetQuestionnaireId(connectionModel, questionnaireName, serverParkName);
            var remoteAuditTrailServer = _auditTrailManagerFactory.GetRemoteAuditTrailServer(connectionModel);
            var auditEvents = remoteAuditTrailServer.GetInstrumentEvents(instrumentId, serverParkName) as IInstrumentEvents2;

            if (auditEvents == null)
            {
                return new List<AuditTrailDataModel>();
            }

            return MapAuditEvents(auditEvents);
        }

        private static List<AuditTrailDataModel> MapAuditEvents(IInstrumentEvents2 auditEvents)
        {
            var listOfAuditEvents = new List<AuditTrailDataModel>();
            var keyValues = auditEvents.GetKeyValues();

            foreach (var keyVal in keyValues)
            {
                var sessionIdAttachedToKey = auditEvents.GetSessionIDsByKeyValue(keyVal);

                foreach (var sessionId in sessionIdAttachedToKey)
                {
                    var sessionEvents = auditEvents.GetSessionEvents(sessionId);

                    foreach (var sessionEvent in sessionEvents.Events)
                    {
                        var auditTrailData = MapAuditTrailData(keyVal, sessionId, sessionEvent);
                        listOfAuditEvents.Add(auditTrailData);
                    }
                }
            }

            return listOfAuditEvents;
        }

        private static AuditTrailDataModel MapAuditTrailData(string keyValue, Guid sessionId, IEventInfo eventInfo)
        {
            return new AuditTrailDataModel
            {
                KeyValue = keyValue,
                SessionId = sessionId,
                TimeStamp = eventInfo.TimeStamp,
                Content = eventInfo.ToString()
            };
        }

        public string GenerateCsvContent(List<AuditTrailDataModel> listOfEvents)
        {
            var csvContent = new StringBuilder();
            csvContent.AppendLine("KeyValue,SessionId,Timestamp,Content");

            foreach (var eventFromAudit in listOfEvents.OrderBy(o => o.KeyValue))
            {
                csvContent.AppendLine($"{eventFromAudit.KeyValue}, {eventFromAudit.SessionId}, {eventFromAudit.TimeStamp.ToString("dd/MM/yyyy HH:mm:ss")}, {eventFromAudit.Content}");
            }
            return csvContent.ToString();
        }
    }
}
