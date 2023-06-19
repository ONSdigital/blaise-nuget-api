using Blaise.Nuget.Api.Contracts.Interfaces;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Providers;
using Blaise.Nuget.Api.Providers;
using System.Security;
using ATA = StatNeth.Blaise.API.AuditTrail;


namespace Blaise.Nuget.Api.Api
{
    public class BlaiseAuditTrailApi : IBlaiseAuditTrailApi
    {
        private readonly ConnectionModel _connectionModel;

        public BlaiseAuditTrailApi(ConnectionModel connection)
        {
            _connectionModel = connection;
        }

        public void GetAuditTrail()
        {
            if (_connectionModel is null)
            {

            }

            var configurationProvider = UnityProvider.Resolve<IBlaiseConfigurationProvider>();
            var password = GetPassword(configurationProvider.GetConnectionModel().Password);
            var ras =
               ATA.AuditTrailManager.GetRemoteAuditTrailServer(
                   configurationProvider.GetConnectionModel().ServerName,
                   configurationProvider.GetConnectionModel().Port,
                   configurationProvider.GetConnectionModel().Username,
                   password);
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

