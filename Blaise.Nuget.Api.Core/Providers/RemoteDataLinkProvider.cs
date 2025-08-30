namespace Blaise.Nuget.Api.Core.Providers
{
    using Blaise.Nuget.Api.Contracts.Models;
    using Blaise.Nuget.Api.Core.Equality;
    using Blaise.Nuget.Api.Core.Extensions;
    using Blaise.Nuget.Api.Core.Interfaces.Factories;
    using Blaise.Nuget.Api.Core.Interfaces.Providers;
    using Blaise.Nuget.Api.Core.Interfaces.Services;
    using StatNeth.Blaise.API.DataLink;
    using System;
    using System.Collections.Generic;

    public class RemoteDataLinkProvider : IRemoteDataLinkProvider
    {
        private readonly IRemoteDataServerFactory _connectionFactory;

        private readonly IQuestionnaireService _questionnaireService;

        private readonly Dictionary<Tuple<string, string, DateTime>, Tuple<IDataLink4, DateTime>> _dataLinkConnections;

        public RemoteDataLinkProvider(
            IRemoteDataServerFactory connectionFactory,
            IQuestionnaireService questionnaireService)
        {
            _connectionFactory = connectionFactory;
            _questionnaireService = questionnaireService;

            _dataLinkConnections = new Dictionary<Tuple<string, string, DateTime>, Tuple<IDataLink4, DateTime>>(new RemoteDataLinkKeyComparison());
        }

        public IDataLink6 GetDataLink(ConnectionModel connectionModel, string questionnaireName, string serverParkName)
        {
            var installDate = _questionnaireService.GetInstallDate(connectionModel, questionnaireName, serverParkName);

            if (!_dataLinkConnections.ContainsKey(new Tuple<string, string, DateTime>(questionnaireName, serverParkName, installDate)))
            {
                return GetFreshConnection(connectionModel, questionnaireName, serverParkName, installDate);
            }

            var (dataLink, expiryDate) =
                _dataLinkConnections[new Tuple<string, string, DateTime>(questionnaireName, serverParkName, installDate)];

            if (!expiryDate.HasExpired() && dataLink != null)
            {
                return dataLink as IDataLink6;
            }

            return GetFreshConnection(connectionModel, questionnaireName, serverParkName, installDate);
        }

        private IDataLink6 GetFreshConnection(
            ConnectionModel connectionModel,
            string questionnaireName,
            string serverParkName,
            DateTime installDate)
        {
            RemoveDeadConnections(questionnaireName, serverParkName);

            var questionnaireId = _questionnaireService.GetQuestionnaireId(connectionModel, questionnaireName, serverParkName);
            var connection = _connectionFactory.GetConnection(connectionModel);
            var dataLink = connection.GetDataLink(questionnaireId, serverParkName);

            _dataLinkConnections[new Tuple<string, string, DateTime>(questionnaireName, serverParkName, installDate)] = null;
            _dataLinkConnections[new Tuple<string, string, DateTime>(questionnaireName, serverParkName, installDate)] =
                new Tuple<IDataLink4, DateTime>(dataLink, connectionModel.ConnectionExpiresInMinutes.GetExpiryDate());

            return dataLink as IDataLink6;
        }

        private void RemoveDeadConnections(string questionnaireName, string serverParkName)
        {
            var deadConnectionKeys = new List<Tuple<string, string, DateTime>>();

            foreach (var entry in _dataLinkConnections)
            {
                if (entry.Key.Item1 == questionnaireName && entry.Key.Item2 == serverParkName)
                {
                    deadConnectionKeys.Add(entry.Key);
                }
            }

            foreach (var deadConnectionKey in deadConnectionKeys)
            {
                _dataLinkConnections[deadConnectionKey] = null;
                _dataLinkConnections.Remove(deadConnectionKey);
            }
        }
    }
}
