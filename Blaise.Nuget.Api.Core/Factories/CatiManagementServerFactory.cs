using System;
using System.Collections.Generic;
using System.Linq;
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

        private readonly Dictionary<string, Tuple<IRemoteCatiManagementServer, DateTime>> _remoteDataServers;

        public CatiManagementServerFactory(IPasswordService passwordService)
        {
            _passwordService = passwordService;
            _remoteDataServers = new Dictionary<string, Tuple<IRemoteCatiManagementServer, DateTime>>(StringComparer.OrdinalIgnoreCase);
        }

        public IRemoteCatiManagementServer GetConnection(ConnectionModel connectionModel)
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

        public int GetOpenConnections()
        {
            return _remoteDataServers.Count;
        }

        public IEnumerable<DateTime> GetExpirationDateTimes()
        {
            return _remoteDataServers.Select(remoteDataServer => remoteDataServer.Value.Item2);
        }

        private IRemoteCatiManagementServer GetFreshServerConnection(ConnectionModel connectionModel)
        {
            var remoteConnection = CreateRemoteConnection(connectionModel);

            _remoteDataServers[connectionModel.ServerName] =
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
