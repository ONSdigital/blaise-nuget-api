﻿using System;
using System.Collections.Generic;
using System.Linq;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Extensions;
using Blaise.Nuget.Api.Core.Interfaces.Factories;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using StatNeth.Blaise.API.DataLink;

namespace Blaise.Nuget.Api.Core.Factories
{
    public class RemoteDataServerFactory : IRemoteDataServerFactory
    {
        private readonly IPasswordService _passwordService;

        private readonly Dictionary<string, Tuple<IRemoteDataServer, DateTime>> _connections;

        public RemoteDataServerFactory(IPasswordService passwordService)
        {
            _passwordService = passwordService;
            _connections = new Dictionary<string, Tuple<IRemoteDataServer, DateTime>>(StringComparer.OrdinalIgnoreCase);
        }

        public IRemoteDataServer GetConnection(ConnectionModel connectionModel)
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

        public void ResetConnections()
        {
            _connections.Clear();
        }

        public int GetNumberOfOpenConnections()
        {
            return _connections.Count;
        }

        public Dictionary<string, DateTime> GetConnections()
        {
            return _connections.ToDictionary(item => item.Key, item => item.Value.Item2);
        }

        private IRemoteDataServer GetFreshServerConnection(ConnectionModel connectionModel)
        {
            var remoteConnection = CreateRemoteConnection(connectionModel);

            _connections[connectionModel.ServerName] =
                new Tuple<IRemoteDataServer, DateTime>(remoteConnection, connectionModel.ConnectionExpiresInMinutes.GetExpiryDate());

            return remoteConnection;
        }

        private IRemoteDataServer CreateRemoteConnection(ConnectionModel connectionModel)
        {
            var securePassword = _passwordService.CreateSecurePassword(connectionModel.Password);

            return DataLinkManager.GetRemoteDataServer(
                connectionModel.ServerName,
                connectionModel.RemotePort,
                connectionModel.Binding,
                connectionModel.UserName,
                securePassword);
        }
    }
}
