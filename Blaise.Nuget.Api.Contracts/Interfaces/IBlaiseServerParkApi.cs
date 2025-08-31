namespace Blaise.Nuget.Api.Contracts.Interfaces
{
    using System.Collections.Generic;
    using StatNeth.Blaise.API.ServerManager;

    public interface IBlaiseServerParkApi
    {
        IServerPark GetServerPark(string serverParkName);

        IEnumerable<IServerPark> GetServerParks();

        IEnumerable<string> GetNamesOfServerParks();

        bool ServerParkExists(string serverParkName);
    }
}
