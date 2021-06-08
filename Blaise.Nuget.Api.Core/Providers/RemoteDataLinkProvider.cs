﻿using System;
using System.Collections.Generic;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Extensions;
using Blaise.Nuget.Api.Core.Interfaces.Factories;
using Blaise.Nuget.Api.Core.Interfaces.Providers;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using StatNeth.Blaise.API.DataLink;

namespace Blaise.Nuget.Api.Core.Providers
{
    public class RemoteDataLinkProvider : IRemoteDataLinkProvider
    {
        private readonly IRemoteDataServerFactory _connectionFactory;
        private readonly ISurveyService _surveyService;

        private readonly Dictionary<Tuple<string, string, DateTime>, Tuple<IDataLink4, DateTime>> _dataLinkConnections;
        
        public RemoteDataLinkProvider(
            IRemoteDataServerFactory connectionFactory,
            ISurveyService surveyService)
        {
            _connectionFactory = connectionFactory;
            _surveyService = surveyService;

            _dataLinkConnections = new Dictionary<Tuple<string, string, DateTime>, Tuple<IDataLink4, DateTime>>();
        }

        public IDataLink4 GetDataLink(ConnectionModel connectionModel, string instrumentName, string serverParkName)
        {
            var installDate = _surveyService.GetInstallDate(connectionModel, instrumentName, serverParkName);

            if (!_dataLinkConnections.ContainsKey(new Tuple<string, string, DateTime>(instrumentName, serverParkName, installDate)))
            {
                return GetFreshConnection(connectionModel, instrumentName, serverParkName, installDate);
            }

            var (dataLink, expiryDate) = 
                _dataLinkConnections[new Tuple<string, string, DateTime>(instrumentName, serverParkName, installDate)];

            return expiryDate.HasExpired()
                ? GetFreshConnection(connectionModel, instrumentName, serverParkName, installDate)
                : dataLink ?? GetFreshConnection(connectionModel, instrumentName, serverParkName, installDate);
        }

        public void ResetConnections()
        {
            _dataLinkConnections.Clear();
        }
        
        public int NumberOfConnections()
        {
            return _dataLinkConnections.Count;
        }

        private IDataLink4 GetFreshConnection(ConnectionModel connectionModel, string instrumentName, string serverParkName,
            DateTime installDate)
        {
            var instrumentId = _surveyService.GetInstrumentId(connectionModel, instrumentName, serverParkName);
            var connection = _connectionFactory.GetConnection(connectionModel);
            var dataLink = connection.GetDataLink(instrumentId, serverParkName);
            var dictionaryEntry = new Tuple<IDataLink4, DateTime>(dataLink, connectionModel.ConnectionExpiresInMinutes.GetExpiryDate());

            _dataLinkConnections[new Tuple<string, string, DateTime>(instrumentName, serverParkName, installDate)] = dictionaryEntry;

            return dataLink;
        }
    }
}
