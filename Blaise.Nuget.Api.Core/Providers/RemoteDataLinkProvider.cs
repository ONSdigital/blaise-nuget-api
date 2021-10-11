using System;
using System.Collections.Generic;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Equality;
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
            Console.WriteLine("RemoteDataLinkProvider - Initialise");
            _connectionFactory = connectionFactory;
            _surveyService = surveyService;

            _dataLinkConnections = new Dictionary<Tuple<string, string, DateTime>, Tuple<IDataLink4, DateTime>>(new RemoteDataLinkKeyComparison());
        }

        public IDataLink4 GetDataLink(ConnectionModel connectionModel, string instrumentName, string serverParkName)
        {
            Console.WriteLine("RemoteDataLinkProvider - GetDataLink");
            var installDate = _surveyService.GetInstallDate(connectionModel, instrumentName, serverParkName);

            if (!_dataLinkConnections.ContainsKey(new Tuple<string, string, DateTime>(instrumentName, serverParkName, installDate)))
            {
                Console.WriteLine("RemoteDataLinkProvider - No Cache found");
                return GetFreshConnection(connectionModel, instrumentName, serverParkName, installDate);
            }

            var (dataLink, expiryDate) = 
                _dataLinkConnections[new Tuple<string, string, DateTime>(instrumentName, serverParkName, installDate)];


            if (!expiryDate.HasExpired() && dataLink != null)
            {
                Console.WriteLine("RemoteDataLinkProvider -  Return cached connection");
                return dataLink;
            }

            Console.WriteLine("RemoteDataLinkProvider - Return fresh connection");
            return GetFreshConnection(connectionModel, instrumentName, serverParkName, installDate);
        }

        private IDataLink4 GetFreshConnection(ConnectionModel connectionModel, string instrumentName, string serverParkName,
            DateTime installDate)
        {
            RemoveDeadConnections(instrumentName, serverParkName);

            var instrumentId = _surveyService.GetInstrumentId(connectionModel, instrumentName, serverParkName);
            var connection = _connectionFactory.GetConnection(connectionModel);
            var dataLink = connection.GetDataLink(instrumentId, serverParkName);
            var dictionaryEntry = new Tuple<IDataLink4, DateTime>(dataLink, connectionModel.ConnectionExpiresInMinutes.GetExpiryDate());
            
            _dataLinkConnections[new Tuple<string, string, DateTime>(instrumentName, serverParkName, installDate)] = dictionaryEntry;

            return dataLink;
        }

        private void RemoveDeadConnections(string instrumentName, string serverParkName)
        {
            var deadConnectionKeys = new List<Tuple<string, string, DateTime>>();

            foreach (var entry in _dataLinkConnections)
            {
                if (entry.Key.Item1 == instrumentName && entry.Key.Item2 == serverParkName)
                {
                    deadConnectionKeys.Add(entry.Key);
                }
            }

            foreach (var deadConnectionKey in deadConnectionKeys)
            {
                _dataLinkConnections.Remove(deadConnectionKey);
            }
        }
    }
}
