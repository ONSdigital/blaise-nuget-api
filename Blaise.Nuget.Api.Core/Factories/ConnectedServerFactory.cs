namespace Blaise.Nuget.Api.Core.Factories
{
    using System;
    using System.Collections.Concurrent;
    using Blaise.Nuget.Api.Contracts.Models;
    using Blaise.Nuget.Api.Core.Extensions;
    using Blaise.Nuget.Api.Core.Interfaces.Factories;
    using Blaise.Nuget.Api.Core.Interfaces.Services;
    using StatNeth.Blaise.API.ServerManager;

    public class ConnectedServerFactory : IConnectedServerFactory
    {
        private readonly IPasswordService _passwordService;
        private readonly ConcurrentDictionary<string, Tuple<IConnectedServer, DateTime>> _connections;

        public ConnectedServerFactory(IPasswordService passwordService)
        {
            _passwordService = passwordService;
            _connections = new ConcurrentDictionary<string, Tuple<IConnectedServer, DateTime>>(StringComparer.OrdinalIgnoreCase);
        }

        public IConnectedServer GetIsolatedConnection(ConnectionModel connectionModel)
        {
            if (_connections.TryGetValue(connectionModel.UserName, out var connection) &&
                !connection.Item2.HasExpired())
            {
                return connection.Item1;
            }

            var connectedServer = GetConnection(connectionModel);
            var expiry = DateTime.Now.AddMinutes(connectionModel.ConnectionExpiresInMinutes);
            _connections.AddOrUpdate(connectionModel.UserName,
                new Tuple<IConnectedServer, DateTime>(connectedServer, expiry),
                (key, old) => new Tuple<IConnectedServer, DateTime>(connectedServer, expiry));

            return connectedServer;
        }

        public IConnectedServer GetConnection(ConnectionModel connectionModel)
        {
            return GetConnection(connectionModel.ServerName, connectionModel.UserName, connectionModel.Password,
                connectionModel.Binding, connectionModel.Port);
        }

        private IConnectedServer GetConnection(string serverName, string userName, string password, string binding, int port)
        {
            var connection = ClientFactory.CreateConnection(
                serverName,
                port,
                binding,
                userName,
                password);

            return connection;
        }
    }
}