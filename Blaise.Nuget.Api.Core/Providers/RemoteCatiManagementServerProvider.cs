namespace Blaise.Nuget.Api.Core.Providers
{
    using Blaise.Nuget.Api.Contracts.Models;
    using Blaise.Nuget.Api.Core.Interfaces.Factories;
    using Blaise.Nuget.Api.Core.Interfaces.Providers;
    using StatNeth.Blaise.API.Cati.Runtime;

    public class RemoteCatiManagementServerProvider : IRemoteCatiManagementServerProvider
    {
        private readonly ICatiManagementServerFactory _catiServerFactory;

        public RemoteCatiManagementServerProvider(ICatiManagementServerFactory catiServerFactory)
        {
            _catiServerFactory = catiServerFactory;
        }

        public IRemoteCatiManagementServer GetCatiManagementForServerPark(ConnectionModel connectionModel, string serverParkName)
        {
            var catiManagement = _catiServerFactory.GetConnection(connectionModel);

            catiManagement.SelectServerPark(serverParkName);

            return catiManagement;
        }
    }
}
