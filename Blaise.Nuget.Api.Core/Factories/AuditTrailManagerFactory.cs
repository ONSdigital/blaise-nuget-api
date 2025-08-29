using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Factories;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using StatNeth.Blaise.API.AuditTrail;

namespace Blaise.Nuget.Api.Core.Factories
{
    public class AuditTrailManagerFactory : IAuditTrailManagerFactory
    {
        private readonly IPasswordService _passwordService;

        public AuditTrailManagerFactory(IPasswordService passwordService)
        {
            _passwordService = passwordService;
        }

        /// <inheritdoc/>
        public IRemoteAuditTrailServer GetRemoteAuditTrailServer(ConnectionModel connectionModel)
        {
            return AuditTrailManager.GetRemoteAuditTrailServer(
                connectionModel.ServerName,
                connectionModel.RemotePort,
                connectionModel.UserName,
                _passwordService.CreateSecurePassword(connectionModel.Password));
        }
    }
}
