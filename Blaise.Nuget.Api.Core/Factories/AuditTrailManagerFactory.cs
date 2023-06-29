using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Factories;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using StatNeth.Blaise.API.AuditTrail;

namespace Blaise.Nuget.Api.Core.Factories
{
    public class AuditTrailManagerFactory: IAuditTrailManagerFactory
    {
        private IPasswordService _passwordService;

        public AuditTrailManagerFactory(IPasswordService passwordService)
        {
            _passwordService = passwordService;
        }

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
