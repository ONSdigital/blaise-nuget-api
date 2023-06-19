using Blaise.Nuget.Api.Contracts.Interfaces;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Providers;
using Blaise.Nuget.Api.Providers;
using System;
using System.Security;
using ATA = StatNeth.Blaise.API.AuditTrail;


namespace Blaise.Nuget.Api.Api
{
    public class BlaiseAuditTrailApi : IBlaiseAuditTrailApi
    {
        private ConnectionModel _connectionModel;

        public BlaiseAuditTrailApi(ConnectionModel connection)
        {
            _connectionModel = connection;
        }

        public void GetAuditTrail()
        {
            var configurationProvider = UnityProvider.Resolve<IBlaiseConfigurationProvider>();

            if (_connectionModel is null)
            {
                _connectionModel = configurationProvider.GetConnectionModel();
            }

            var password = GetPassword(configurationProvider.GetConnectionModel().Password);
            var ras =
               ATA.AuditTrailManager.GetRemoteAuditTrailServer(
                   configurationProvider.GetConnectionModel().ServerName,
                   configurationProvider.GetConnectionModel().Port,
                   configurationProvider.GetConnectionModel().UserName,
                   password);

            ATA.IInstrumentEvents instEvents =
                         ras.GetInstrumentEvents(Guid.Parse("some guid value"),
                                                 "gusty");

            if (instEvents != null)
            {
                // some stuff here
            }
        }

            private static SecureString GetPassword(string pw)
        {
            char[] passwordChars = pw.ToCharArray();
            var password = new SecureString();

            foreach (var c in passwordChars)
            {
                password.AppendChar(c);
            }
            return password;
        }


    }
}

