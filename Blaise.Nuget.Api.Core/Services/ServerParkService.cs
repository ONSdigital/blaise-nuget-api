namespace Blaise.Nuget.Api.Core.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Blaise.Nuget.Api.Contracts.Models;
    using Blaise.Nuget.Api.Core.Interfaces.Factories;
    using Blaise.Nuget.Api.Core.Interfaces.Services;
    using StatNeth.Blaise.API.ServerManager;
    
    public class ServerParkService : IServerParkService
    {
        private readonly IConnectedServerFactory _connectedServerFactory;

        public ServerParkService(IConnectedServerFactory connectedServerFactory)
        {
            _connectedServerFactory = connectedServerFactory;
        }

        public IEnumerable<IServerPark> GetServerParks(ConnectionModel connectionModel)
        {
            var connection = _connectedServerFactory.GetConnection(connectionModel);

            return connection.ServerParks;
        }

        public IServerPark GetServerPark(ConnectionModel connectionModel, string serverParkName)
        {
            var connection = _connectedServerFactory.GetConnection(connectionModel);

            return connection.GetServerPark(serverParkName);
        }

        public IEnumerable<string> GetServerParkNames(ConnectionModel connectionModel)
        {
            var serverParks = GetServerParks(connectionModel);

            return serverParks.Select(sp => sp.Name);
        }
    }
}
