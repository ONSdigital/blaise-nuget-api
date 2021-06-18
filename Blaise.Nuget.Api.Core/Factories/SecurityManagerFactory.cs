using System;
using System.Collections.Generic;
using System.Linq;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Extensions;
using Blaise.Nuget.Api.Core.Interfaces.Factories;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using StatNeth.Blaise.API.Security;
using StatNeth.Blaise.Shared.API;

namespace Blaise.Nuget.Api.Core.Factories
{
    public class SecurityManagerFactory : ISecurityManagerFactory
    {
        private readonly IPasswordService _passwordService;

        private readonly Dictionary<string, Tuple<ISecurityServer, DateTime>> _securityServers;

        public SecurityManagerFactory(IPasswordService passwordService)
        {
            _passwordService = passwordService;
            _securityServers = new Dictionary<string, Tuple<ISecurityServer, DateTime>>(StringComparer.OrdinalIgnoreCase);
        }

        public ISecurityServer GetConnection(ConnectionModel connectionModel)
        {
            if (!_securityServers.ContainsKey(connectionModel.ServerName))
            {
                return GetFreshServerConnection(connectionModel);

            }
            
            var (remoteServer, expiryDate) = _securityServers[connectionModel.ServerName];

            return expiryDate.HasExpired()
                ? GetFreshServerConnection(connectionModel)
                : remoteServer ?? GetFreshServerConnection(connectionModel);
        }

        public void ResetConnections()
        {
            _securityServers.Clear();
        }

        public int GetNumberOfOpenConnections()
        {
            return _securityServers.Count;
        }

        public Dictionary<string, DateTime> GetConnections()
        {
            return _securityServers.ToDictionary(item => item.Key, item => item.Value.Item2);
        }

        private ISecurityServer GetFreshServerConnection(ConnectionModel connectionModel)
        {
            var securityConnection = CreateConnection(connectionModel);

            _securityServers[connectionModel.ServerName] =
                new Tuple<ISecurityServer, DateTime>(securityConnection, connectionModel.ConnectionExpiresInMinutes.GetExpiryDate());

            return securityConnection;
        }

        private ISecurityServer CreateConnection(ConnectionModel connectionModel)
        {
            var securePassword = _passwordService.CreateSecurePassword(connectionModel.Password);

            return SecurityManager.Connect(
                connectionModel.ServerName,
                connectionModel.Port,
                connectionModel.UserName,
                securePassword,
                connectionModel.Binding.ToEnum<ClientPreferredBinding>());
        }
    }
}
