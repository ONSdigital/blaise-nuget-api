using System;
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

        private readonly Dictionary<string, Tuple<IRemoteDataServer, DateTime>> _remoteDataServers;

        public RemoteDataServerFactory(IPasswordService passwordService)
        {
            _passwordService = passwordService;
            _remoteDataServers = new Dictionary<string, Tuple<IRemoteDataServer, DateTime>>(StringComparer.OrdinalIgnoreCase);
        }

        public IRemoteDataServer GetConnection(ConnectionModel connectionModel)
        {
            if (!_remoteDataServers.ContainsKey(connectionModel.ServerName))
            {
                return GetFreshServerConnection(connectionModel);
            }

            var (remoteServer, expiryDate) = _remoteDataServers[connectionModel.ServerName];

            return expiryDate.HasExpired()
                ? GetFreshServerConnection(connectionModel)
                : remoteServer ?? GetFreshServerConnection(connectionModel);
        }

        public void ResetConnections()
        {
            _remoteDataServers.Clear();
        }

        private IRemoteDataServer GetFreshServerConnection(ConnectionModel connectionModel)
        {
            var remoteConnection = CreateRemoteConnection(connectionModel);

            _remoteDataServers[connectionModel.ServerName] =
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
