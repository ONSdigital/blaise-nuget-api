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
        private IQuestionnaireService _questionnaireService;
        private IAuditTrailManagerFactory _auditTrailManagerFactory;

        public AuditTrailService(IQuestionnaireService questionnaireService, IAuditTrailManagerFactory auditTrailManagerFactory)
        {
            _questionnaireService = questionnaireService;
            _auditTrailManagerFactory = auditTrailManagerFactory;
        }

        public List<AuditTrailData> GetAuditTrailData(ConnectionModel connectionModel, string questionnaireName, string serverParkName)
        {
            var instrumentId = _questionnaireService.GetQuestionnaireId(connectionModel, questionnaireName, serverParkName);
            var remoteAuditTrailServer = _auditTrailManagerFactory.GetRemoteAuditTrailServer(connectionModel);
            var auditEvents =
                remoteAuditTrailServer.GetInstrumentEvents(instrumentId, serverParkName) as IInstrumentEvents2;
            var listOfAuditEvents = new List<AuditTrailData>();

            if (auditEvents == null)
            {
                return listOfAuditEvents;
            }
            
            var keyValues = auditEvents.GetKeyValues();

            foreach (var keyVal in keyValues)
            {
                var sessionIdAttachedToKey = auditEvents.GetSessionIDsByKeyValue(keyVal);

                foreach (var sessionId in sessionIdAttachedToKey)
                {
                    var sessionEvents = auditEvents.GetSessionEvents(sessionId);

                    
                }
            }

            return listOfAuditEvents;
        }

        private List<AuditTrailData> MapAuditTrailData(ISessionEvents sessionEvents)
        {
            foreach (var sessionEvent in sessionEvents.Events)
            {
                var auditTrailData = new AuditTrailData
                {
                    KeyValue = keyVal,
                    SessionId = sessionId,
                    TimeStamp = sessionEvent.TimeStamp,
                    Content = sessionEvent.ToString()
                };
                listOfAuditEvents.Add(auditTrailData);
            }

        }


        public string GenerateCsvContent(List<AuditTrailData> listOfEvents)
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
