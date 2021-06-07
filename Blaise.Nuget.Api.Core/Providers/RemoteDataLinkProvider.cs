using System;
using System.Collections.Generic;
using System.Linq;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Extensions;
using Blaise.Nuget.Api.Core.Interfaces.Factories;
using Blaise.Nuget.Api.Core.Interfaces.Providers;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using StatNeth.Blaise.API.DataLink;
using StatNeth.Blaise.API.DataRecord;

namespace Blaise.Nuget.Api.Core.Providers
{
    public class RemoteDataLinkProvider : IRemoteDataLinkProvider
    {
        private readonly IRemoteDataServerFactory _connectionFactory;
        private readonly ISurveyService _surveyService;

        private readonly Dictionary<Tuple<string, string>, Tuple<IDataLink4, DateTime>> _dataLinkConnections;
        
        public RemoteDataLinkProvider(
            IRemoteDataServerFactory connectionFactory,
            ISurveyService surveyService)
        {
            _connectionFactory = connectionFactory;
            _surveyService = surveyService;

            _dataLinkConnections = new Dictionary<Tuple<string, string>, Tuple<IDataLink4, DateTime>>();
        }

        public void LockDataRecord(ConnectionModel connectionModel, string instrumentName, string serverParkName,
            IKey primaryKey, string lockId)
        {
            var dataLink = GetDataLink(connectionModel, instrumentName, serverParkName);

            dataLink.Lock(primaryKey, lockId);
        }

        public void UnLockDataRecord(ConnectionModel connectionModel, string instrumentName, string serverParkName,
            IKey primaryKey, string lockId)
        {
            var dataLink = GetDataLink(connectionModel, instrumentName, serverParkName);

            dataLink.Unlock(primaryKey, lockId);
        }

        public IDataLink4 GetDataLink(ConnectionModel connectionModel, string instrumentName, string serverParkName)
        {
            if (!_dataLinkConnections.ContainsKey(new Tuple<string, string>(instrumentName, serverParkName)))
            {
                return GetFreshConnection(connectionModel, instrumentName, serverParkName);
            }

            var (dataLink, expiryDate) = _dataLinkConnections[new Tuple<string, string>(instrumentName, serverParkName)];

            return expiryDate.HasExpired()
                ? GetFreshConnection(connectionModel, instrumentName, serverParkName)
                : dataLink ?? GetFreshConnection(connectionModel, instrumentName, serverParkName);
        }

        public void ResetConnections()
        {
            _dataLinkConnections.Clear();
        }

        private IDataLink4 GetFreshConnection(ConnectionModel connectionModel, string instrumentName, string serverParkName)
        {
            var instrumentId = _surveyService.GetInstrumentId(connectionModel, instrumentName, serverParkName);
            var connection = _connectionFactory.GetConnection(connectionModel);
            var dataLink = connection.GetDataLink(instrumentId, serverParkName);
            var dictionaryEntry = new Tuple<IDataLink4, DateTime>(dataLink, connectionModel.ConnectionExpiresInMinutes.GetExpiryDate());

            _dataLinkConnections[new Tuple<string, string>(instrumentName, serverParkName)] = dictionaryEntry;

            return dataLink;
        }
    }
}
