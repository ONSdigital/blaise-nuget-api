using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Factories;
using Blaise.Nuget.Api.Core.Interfaces.Providers;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using StatNeth.Blaise.API.AuditTrail;
using StatNeth.Blaise.API.DataEntry.ServerPark;
using static StatNeth.Blaise.Meta.Parsing.MetaObjectTokenCollection;

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

        public byte[] GetAuditTrailData(ConnectionModel connectionModel, string questionnaireName, string serverParkName)
        {
            var instrumentId = _questionnaireService.GetQuestionnaireId(connectionModel, questionnaireName, serverParkName);
            var remoteAuditTrailServer = _auditTrailManagerFactory.GetRemoteAuditTrailServer(connectionModel);
            var auditEvents =
                remoteAuditTrailServer.GetInstrumentEvents(instrumentId, serverParkName) as IInstrumentEvents2;

            if (auditEvents == null)
            {
                return Array.Empty<byte>();
            }

            var listOfAuditEvents = new List<AuditTrailData>();
            var keyValues = auditEvents.GetKeyValues();

            foreach (var keyVal in keyValues)
            {
                var sessionIdAttachedToKey = auditEvents.GetSessionIDsByKeyValue(keyVal);

                foreach (var sessionId in sessionIdAttachedToKey)
                {
                    var sessionEvents = auditEvents.GetSessionEvents(sessionId);

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
            }

            if (!listOfAuditEvents.Any())
            {
                return Array.Empty<byte>();
            }

            var csvContent = GenerateCsvContent(listOfAuditEvents);
            return GenerateFileInMemory(csvContent);
        }

        private string GenerateCsvContent(List<AuditTrailData> listOfEvents)
        {
            var csvContent = new StringBuilder();
            csvContent.AppendLine("KeyValue,SessionId,Timestamp,Content");

            foreach (var eventFromAudit in listOfEvents.OrderBy(o => o.KeyValue))
            {
                csvContent.AppendLine($"{eventFromAudit.KeyValue}, {eventFromAudit.SessionId}, {eventFromAudit.TimeStamp.ToString("dd/MM/yyyy HH:mm:ss")}, {eventFromAudit.Content}");
            }
            return csvContent.ToString();
        }

        private byte[] GenerateFileInMemory(string csvContent)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var streamWriter = new StreamWriter(memoryStream))
                {
                    streamWriter.Write(csvContent);
                    streamWriter.Flush();

                    // Rewind the MemoryStream
                    memoryStream.Position = 0;

                    //Save as byte array
                    return memoryStream.ToArray();
                }
            }
        }
    }
}
