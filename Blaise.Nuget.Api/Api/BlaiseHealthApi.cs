using Blaise.Nuget.Api.Contracts.Interfaces;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Factories;
using Blaise.Nuget.Api.Core.Interfaces.Providers;
using Blaise.Nuget.Api.Providers;

namespace Blaise.Nuget.Api.Api
{
    public class BlaiseHealthApi : IBlaiseHealthApi
    {
        private readonly IConnectedServerFactory _connectedServerFactory;
        private readonly IRemoteDataServerFactory _remoteDataServerFactory;
        private readonly ICatiManagementServerFactory _catiManagementServerFactory;

        private readonly ConnectionModel _connectionModel;

        internal BlaiseHealthApi(
            IConnectedServerFactory connectedServerFactory,
            IRemoteDataServerFactory remoteDataServerFactory,
            ICatiManagementServerFactory catiManagementServerFactory,
            ConnectionModel connectionModel)
        {
            _connectedServerFactory = connectedServerFactory;
            _remoteDataServerFactory = remoteDataServerFactory;
            _catiManagementServerFactory = catiManagementServerFactory;
            _connectionModel = connectionModel;
        }

        public BlaiseHealthApi(ConnectionModel connectionModel = null)
        {
            _connectedServerFactory = UnityProvider.Resolve<IConnectedServerFactory>();
            _remoteDataServerFactory = UnityProvider.Resolve<IRemoteDataServerFactory>();
            _catiManagementServerFactory = UnityProvider.Resolve<ICatiManagementServerFactory>();

            var configurationProvider = UnityProvider.Resolve<IBlaiseConfigurationProvider>();
            _connectionModel = connectionModel ?? configurationProvider.GetConnectionModel();
        }

        public bool ConnectionModelIsHealthy()
        {
            return !string.IsNullOrWhiteSpace(_connectionModel.ServerName) &&
                   !string.IsNullOrWhiteSpace(_connectionModel.UserName) &&
                   !string.IsNullOrWhiteSpace(_connectionModel.Password) &&
                   !string.IsNullOrWhiteSpace(_connectionModel.Binding) &&
                   _connectionModel.Port > 0 &&
                   _connectionModel.RemotePort > 0;
        }

        public bool ConnectionToBlaiseIsHealthy()
        {
            try
            {
                _connectedServerFactory.GetConnection(_connectionModel);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool RemoteConnectionToBlaiseIsHealthy()
        {
            try
            {
                _remoteDataServerFactory.GetConnection(_connectionModel);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool RemoteConnectionToCatiIsHealthy()
        {
            try
            {
                _catiManagementServerFactory.GetConnection(_connectionModel);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
