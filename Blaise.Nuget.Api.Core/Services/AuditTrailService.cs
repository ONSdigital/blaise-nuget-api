using System.Collections.Generic;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Factories;
using Blaise.Nuget.Api.Core.Interfaces.Mappers;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using StatNeth.Blaise.API.AuditTrail;

namespace Blaise.Nuget.Api.Core.Services
{
    public class AuditTrailService : IAuditTrailService
    {
        private readonly IQuestionnaireService _questionnaireService;
        private readonly IAuditTrailManagerFactory _auditTrailManagerFactory;
        private readonly IAuditTrailDataMapper _auditTrailDataMapper;

        public AuditTrailService(
            IQuestionnaireService questionnaireService, 
            IAuditTrailManagerFactory auditTrailManagerFactory, 
            IAuditTrailDataMapper auditTrailDataMapper)
        {
            _questionnaireService = questionnaireService;
            _auditTrailManagerFactory = auditTrailManagerFactory;
            _auditTrailDataMapper = auditTrailDataMapper;
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

            return CreateAuditTrailDataFromEvents(auditEvents);
        }

        public string GenerateCsvContent(List<AuditTrailDataModel> listOfEvents)
        {
            return _auditTrailDataMapper.MapAuditTrailCsvContent(listOfEvents);
        }

        private List<AuditTrailDataModel> CreateAuditTrailDataFromEvents(IInstrumentEvents2 auditEvents)
        {
            var auditTrailDataModels = new List<AuditTrailDataModel>();
            var keyValues = auditEvents.GetKeyValues();

            foreach (var keyVal in keyValues)
            {
                var sessionIdAttachedToKey = auditEvents.GetSessionIDsByKeyValue(keyVal);

                foreach (var sessionId in sessionIdAttachedToKey)
                {
                    var sessionEvents = auditEvents.GetSessionEvents(sessionId);

                    foreach (var sessionEvent in sessionEvents.Events)
                    {
                        var auditTrailDataModel = _auditTrailDataMapper.MapAuditTrailDataModel(keyVal, sessionId, sessionEvent);
                        auditTrailDataModels.Add(auditTrailDataModel);
                    }
                }
            }

            return auditTrailDataModels;
        }
    }
}
