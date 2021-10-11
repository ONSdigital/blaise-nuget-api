using System;
using System.Collections.Generic;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Extensions;
using Blaise.Nuget.Api.Core.Interfaces.Factories;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using StatNeth.Blaise.API.Cati.Runtime;

namespace Blaise.Nuget.Api.Core.Factories
{
    public class CatiManagementServerFactory : ICatiManagementServerFactory
    {
        private readonly IPasswordService _passwordService;

        private readonly Dictionary<string, Tuple<IRemoteCatiManagementServer, DateTime>> _connections;

        public CatiManagementServerFactory(IPasswordService passwordService)
        {
            Console.WriteLine("CatiManagementServerFactory - Initialise");
            _passwordService = passwordService;
            _connections = new Dictionary<string, Tuple<IRemoteCatiManagementServer, DateTime>>(StringComparer.OrdinalIgnoreCase);
        }

        public IRemoteCatiManagementServer GetConnection(ConnectionModel connectionModel)
        {
            Console.WriteLine("CatiManagementServerFactory - GetConnection");

            if (!_connections.ContainsKey(connectionModel.ServerName))
            {
                Console.WriteLine("CatiManagementServerFactory - No Cache found");
                return GetFreshServerConnection(connectionModel);
            }

            var (remoteServer, expiryDate) = _connections[connectionModel.ServerName];

            if (!expiryDate.HasExpired() && remoteServer != null)
            {
                Console.WriteLine("CatiManagementServerFactory -  Return cached connection");
                return remoteServer;

            }
            
            Console.WriteLine("CatiManagementServerFactory - Return fresh connection");
            return GetFreshServerConnection(connectionModel);
        }

        private IRemoteCatiManagementServer GetFreshServerConnection(ConnectionModel connectionModel)
        {
            var remoteConnection = CreateRemoteConnection(connectionModel);

            _connections[connectionModel.ServerName] =
                new Tuple<IRemoteCatiManagementServer, DateTime>(remoteConnection, connectionModel.ConnectionExpiresInMinutes.GetExpiryDate());

            return remoteConnection;
        }

        private IRemoteCatiManagementServer CreateRemoteConnection(ConnectionModel connectionModel)
        {
            var securePassword = _passwordService.CreateSecurePassword(connectionModel.Password);

            return CatiRuntimeManager.GetRemoteCatiManagementServer(
                connectionModel.Binding,
                connectionModel.ServerName,
                connectionModel.RemotePort,
                connectionModel.UserName,
                securePassword);
        }
    }
}
