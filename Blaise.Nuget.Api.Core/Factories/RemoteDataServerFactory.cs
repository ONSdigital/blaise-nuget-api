﻿using Blaise.Nuget.Api.Core.Interfaces;
using Blaise.Nuget.Api.Core.Models;
using StatNeth.Blaise.API.DataLink;

namespace Blaise.Nuget.Api.Core.Factories
{
    public class RemoteDataServerFactory : IRemoteDataServerFactory
    {
        private readonly IPasswordService _passwordService;
        private readonly ConnectionModel _connectionModel;

        private IRemoteDataServer _remoteDataServer;

        public RemoteDataServerFactory(
            ConnectionModel connectionModel,
            IPasswordService passwordService)
        {
            _connectionModel = connectionModel;
            _passwordService = passwordService;
        }

        public IRemoteDataServer GetConnection()
        {   
            if(_remoteDataServer == null)
            {
                CreateRemoteConnection(_connectionModel);
            }

            return _remoteDataServer;
        }

        private void CreateRemoteConnection(ConnectionModel connectionModel)
        {
            var securePassword = _passwordService.CreateSecurePassword(connectionModel.Password);

            _remoteDataServer = DataLinkManager.GetRemoteDataServer(
                connectionModel.ServerName,
                connectionModel.Port,
                connectionModel.Binding,
                connectionModel.UserName,
                securePassword);
        }
    }
}