using System.Collections.Generic;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Providers;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using Blaise.Nuget.Api.Extensions;
using Blaise.Nuget.Api.Providers;

namespace Blaise.Nuget.Api.Api
{
    public class BlaiseAuditTrailApi : IBlaiseAuditTrailApi
    {
        private readonly IAuditTrailService _auditTrailService;
        private readonly ConnectionModel _connectionModel;

        public BlaiseAuditTrailApi(
            IAuditTrailService auditTrailService,
            ConnectionModel connectionModel)
        {
            _auditTrailService = auditTrailService;
            _connectionModel = connectionModel;
        }

        public BlaiseAuditTrailApi(ConnectionModel connectionModel = null)
        {
            _auditTrailService = UnityProvider.Resolve<IAuditTrailService>();

            var configurationProvider = UnityProvider.Resolve<IBlaiseConfigurationProvider>();
            _connectionModel = connectionModel ?? configurationProvider.GetConnectionModel();
        }

        public IEnumerable<AuditTrailDataModel> GetAuditTrail(string questionnaireName, string serverParkName)
        {
            questionnaireName.ThrowExceptionIfNullOrEmpty("questionnaireName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            return _auditTrailService.GetAuditTrailData(_connectionModel, questionnaireName, serverParkName);
        }
    }
}
