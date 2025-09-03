namespace Blaise.Nuget.Api.Api
{
    using System.Collections.Generic;
    using Blaise.Nuget.Api.Contracts.Interfaces;
    using Blaise.Nuget.Api.Contracts.Models;
    using Blaise.Nuget.Api.Core.Interfaces.Providers;
    using Blaise.Nuget.Api.Core.Interfaces.Services;
    using Blaise.Nuget.Api.Extensions;
    using Blaise.Nuget.Api.Providers;
    using StatNeth.Blaise.API.ServerManager;

    public class BlaiseServerParkApi : IBlaiseServerParkApi
    {
        private readonly IServerParkService _parkService;

        private readonly ConnectionModel _connectionModel;

        public BlaiseServerParkApi(
            IServerParkService parkService,
            ConnectionModel connectionModel)
        {
            _parkService = parkService;
            _connectionModel = connectionModel;
        }

        public BlaiseServerParkApi(ConnectionModel connectionModel = null)
        {
            _parkService = UnityProvider.Resolve<IServerParkService>();

            var configurationProvider = UnityProvider.Resolve<IBlaiseConfigurationProvider>();
            _connectionModel = connectionModel ?? configurationProvider.GetConnectionModel();
        }

        /// <inheritdoc/>
        public IServerPark GetServerPark(string serverParkName)
        {
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            return _parkService.GetServerPark(_connectionModel, serverParkName);
        }

        /// <inheritdoc/>
        public IEnumerable<IServerPark> GetServerParks()
        {
            return _parkService.GetServerParks(_connectionModel);
        }

        /// <inheritdoc/>
        public IEnumerable<string> GetNamesOfServerParks()
        {
            return _parkService.GetServerParkNames(_connectionModel);
        }

        /// <inheritdoc/>
        public bool ServerParkExists(string serverParkName)
        {
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            return _parkService.ServerParkExists(_connectionModel, serverParkName);
        }
    }
}
