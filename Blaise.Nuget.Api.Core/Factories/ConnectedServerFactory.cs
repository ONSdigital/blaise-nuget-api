namespace Blaise.Nuget.Api.Core.Factories
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
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

        public IConnectedServer GetConnection(ConnectionModel connectionModel)
        {
            var connectionTuple = _connections.AddOrUpdate(
                connectionModel.ServerName,
                key => CreateNewConnectionTuple(connectionModel),
                (key, existingTuple) =>
                {
                    if (existingTuple.Item2.HasExpired())
                    {
                        return CreateNewConnectionTuple(connectionModel);
                    }
                    else
                    {
                        return existingTuple;
                    }
                }
            );
            return connectionTuple.Item1;
        }

        public IConnectedServer GetIsolatedConnection(ConnectionModel connectionModel)
        {
            return CreateServerConnection(connectionModel);
        }

        private Tuple<IConnectedServer, DateTime> CreateNewConnectionTuple(ConnectionModel connectionModel)
        {
            var connectedServer = CreateServerConnection(connectionModel);
            var expiryDate = connectionModel.ConnectionExpiresInMinutes.GetExpiryDate();
            return new Tuple<IConnectedServer, DateTime>(connectedServer, expiryDate);
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
