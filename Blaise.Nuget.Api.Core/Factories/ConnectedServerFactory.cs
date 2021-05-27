using System;
using System.Collections.Generic;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Extensions;
using Blaise.Nuget.Api.Core.Interfaces.Factories;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using StatNeth.Blaise.API.ServerManager;

namespace Blaise.Nuget.Api.Core.Factories
{
    public class ConnectedServerFactory : IConnectedServerFactory
    {
        private readonly IPasswordService _passwordService;

        private readonly Dictionary<string, Tuple<IConnectedServer, DateTime>> _connectedServers;

        public ConnectedServerFactory(IPasswordService passwordService)
        {
            _passwordService = passwordService;
            _connectedServers = new Dictionary<string, Tuple<IConnectedServer, DateTime>>(StringComparer.OrdinalIgnoreCase);
        }

        public IConnectedServer GetConnection(ConnectionModel connectionModel)
        {
            if (!_connectedServers.ContainsKey(connectionModel.ServerName))
            {
                return GetFreshServerConnection(connectionModel);
            }
            
            var (connectedServer, expiryDate) = _connectedServers[connectionModel.ServerName];

            return expiryDate.HasExpired()
                ? GetFreshServerConnection(connectionModel)
                : connectedServer ?? GetFreshServerConnection(connectionModel);
        }

        public IConnectedServer GetIsolatedConnection(ConnectionModel connectionModel)
        {
            return CreateServerConnection(connectionModel);
        }

        public void ResetConnections()
        {
            _connectedServers.Clear();
        }

        private IConnectedServer GetFreshServerConnection(ConnectionModel connectionModel)
        {
            var connectedServer = CreateServerConnection(connectionModel);

            _connectedServers[connectionModel.ServerName] =
                new Tuple<IConnectedServer, DateTime>(connectedServer, connectionModel.ConnectionExpiresInMinutes.GetExpiryDate());

            return connectedServer;
        }

        private IConnectedServer CreateServerConnection(ConnectionModel connectionModel)
        {
            return ServerManager.ConnectToServer(
                connectionModel.ServerName,
                connectionModel.Port,
                connectionModel.UserName,
                _passwordService.CreateSecurePassword(connectionModel.Password),
                connectionModel.Binding);
        }
    }
}
