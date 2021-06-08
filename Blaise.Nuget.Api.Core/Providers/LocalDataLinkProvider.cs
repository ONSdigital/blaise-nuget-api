using System;
using System.Collections.Generic;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Extensions;
using Blaise.Nuget.Api.Core.Interfaces.Providers;
using StatNeth.Blaise.API.DataLink;

namespace Blaise.Nuget.Api.Core.Providers
{
    public class LocalDataLinkProvider : ILocalDataLinkProvider
    {
        private readonly Dictionary<string, Tuple<IDataLink4, DateTime>> _dataLinkConnections;

        public LocalDataLinkProvider()
        {
            _dataLinkConnections = new Dictionary<string, Tuple<IDataLink4, DateTime>>(StringComparer.OrdinalIgnoreCase);
        }

        public IDataLink4 GetDataLink(ConnectionModel connectionModel, string databaseFile)
        {
            if (!_dataLinkConnections.ContainsKey(databaseFile))
            {
                return GetFreshConnection(connectionModel, databaseFile);
            }

            var (dataLink, expiryDate) = _dataLinkConnections[databaseFile];

            return expiryDate.HasExpired()
                ? GetFreshConnection(connectionModel, databaseFile)
                : dataLink ?? GetFreshConnection(connectionModel, databaseFile);
        }

        public void ResetConnections()
        {
            _dataLinkConnections.Clear();
        }

        public int GetOpenConnections()
        {
            return _dataLinkConnections.Count;
        }

        private IDataLink4 GetFreshConnection(ConnectionModel connectionModel, string databaseFile)
        {
            var dataLink = DataLinkManager.GetDataLink(databaseFile) as IDataLink4;

            _dataLinkConnections[databaseFile] = new Tuple<IDataLink4, DateTime>(dataLink, connectionModel.ConnectionExpiresInMinutes.GetExpiryDate());

            return dataLink;
        }
    }
}
