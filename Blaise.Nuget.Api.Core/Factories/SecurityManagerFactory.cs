using System;
using System.Collections.Generic;
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

        private readonly Dictionary<string, Tuple<ISecurityServer, DateTime>> _connections;

        public SecurityManagerFactory(IPasswordService passwordService)
        {
            _passwordService = passwordService;
            _connections = new Dictionary<string, Tuple<ISecurityServer, DateTime>>(StringComparer.OrdinalIgnoreCase);
        }

        /// <inheritdoc/>
        public ISecurityServer GetConnection(ConnectionModel connectionModel)
        {
            if (!_connections.ContainsKey(connectionModel.ServerName))
            {
                return GetFreshServerConnection(connectionModel);
            }

            var (remoteServer, expiryDate) = _connections[connectionModel.ServerName];

            return expiryDate.HasExpired()
                ? GetFreshServerConnection(connectionModel)
                : remoteServer ?? GetFreshServerConnection(connectionModel);
        }

        private ISecurityServer GetFreshServerConnection(ConnectionModel connectionModel)
        {
            var securityConnection = CreateConnection(connectionModel);

            _connections[connectionModel.ServerName] = null;
            _connections[connectionModel.ServerName] =
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
