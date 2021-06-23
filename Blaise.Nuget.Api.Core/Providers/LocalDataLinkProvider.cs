using System;
using System.Collections.Generic;
using System.Linq;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Extensions;
using Blaise.Nuget.Api.Core.Interfaces.Providers;
using StatNeth.Blaise.API.DataLink;

namespace Blaise.Nuget.Api.Core.Providers
{
    public class LocalDataLinkProvider : ILocalDataLinkProvider
    {
        private readonly Dictionary<string, Tuple<IDataLink4, DateTime>> _connections;

        public LocalDataLinkProvider()
        {
            _connections = new Dictionary<string, Tuple<IDataLink4, DateTime>>(StringComparer.OrdinalIgnoreCase);
        }

        public IDataLink4 GetDataLink(ConnectionModel connectionModel, string databaseFile)
        {
            if (!_connections.ContainsKey(databaseFile))
            {
                return GetFreshConnection(connectionModel, databaseFile);
            }

            var (dataLink, expiryDate) = _connections[databaseFile];

            return expiryDate.HasExpired()
                ? GetFreshConnection(connectionModel, databaseFile)
                : dataLink ?? GetFreshConnection(connectionModel, databaseFile);
        }

        private IDataLink4 GetFreshConnection(ConnectionModel connectionModel, string databaseFile)
        {
            var dataLink = DataLinkManager.GetDataLink(databaseFile) as IDataLink4;

            _connections[databaseFile] = new Tuple<IDataLink4, DateTime>(dataLink, connectionModel.ConnectionExpiresInMinutes.GetExpiryDate());

            return dataLink;
        }
    }
}
