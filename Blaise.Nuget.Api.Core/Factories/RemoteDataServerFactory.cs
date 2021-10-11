﻿using System;
using System.Collections.Generic;
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
            Console.WriteLine("RemoteDataServerFactory - Initialise");

            _passwordService = passwordService;
            _connections = new Dictionary<string, Tuple<IRemoteDataServer, DateTime>>(StringComparer.OrdinalIgnoreCase);
        }

        public IRemoteDataServer GetConnection(ConnectionModel connectionModel)
        {
            Console.WriteLine("RemoteDataServerFactory - GetConnection");

            if (!_connections.ContainsKey(connectionModel.ServerName))
            {
                Console.WriteLine("RemoteDataServerFactory - No Cache found");
                return GetFreshServerConnection(connectionModel);
            }

            var (remoteServer, expiryDate) = _connections[connectionModel.ServerName];

            if (!expiryDate.HasExpired() && remoteServer != null)
            {
                Console.WriteLine("RemoteDataServerFactory -  Return cached connection");
                return remoteServer;

            }

            Console.WriteLine("RemoteDataServerFactory - Return fresh connection");
            return GetFreshServerConnection(connectionModel);
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
