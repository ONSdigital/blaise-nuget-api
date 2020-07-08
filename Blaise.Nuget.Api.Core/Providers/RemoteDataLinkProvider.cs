﻿using Blaise.Nuget.Api.Core.Interfaces.Factories;
using Blaise.Nuget.Api.Core.Interfaces.Providers;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using StatNeth.Blaise.API.DataLink;

namespace Blaise.Nuget.Api.Core.Providers
{
    public class RemoteDataLinkProvider : IRemoteDataLinkProvider
    {
        private readonly IRemoteDataServerFactory _connectionFactory;
        private readonly ISurveyService _surveyService;
        private readonly IConnectionExpiryService _connectionExpiryService;

        private string _instrumentName;
        private string _serverParkName;
        private IDataLink4 _dataLink;

        public RemoteDataLinkProvider(
            IRemoteDataServerFactory connectionFactory,
            ISurveyService surveyService, 
            IConnectionExpiryService connectionExpiryService)
        {
            _connectionFactory = connectionFactory;
            _surveyService = surveyService;
            _connectionExpiryService = connectionExpiryService;

            _instrumentName = string.Empty;
            _serverParkName = string.Empty;
        }

        public IDataLink4 GetDataLink(string instrumentName, string serverParkName)
        {
            if (_dataLink == null || instrumentName != _instrumentName || serverParkName != _serverParkName 
                || _connectionExpiryService.ConnectionHasExpired())
            {
                _instrumentName = instrumentName;
                _serverParkName = serverParkName;

                var instrumentId = _surveyService.GetInstrumentId(instrumentName, serverParkName);
                var connection = _connectionFactory.GetConnection();

                _dataLink = connection.GetDataLink(instrumentId, serverParkName);
                _connectionExpiryService.ResetConnectionExpiryPeriod();
            }

            return _dataLink;
        }
    }
}
