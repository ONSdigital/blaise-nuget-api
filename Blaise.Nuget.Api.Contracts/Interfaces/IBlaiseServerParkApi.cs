namespace Blaise.Nuget.Api.Contracts.Interfaces
{
    using StatNeth.Blaise.API.ServerManager;
    using System.Collections.Generic;

    public interface IBlaiseServerParkApi
    {
        IServerPark GetServerPark(string serverParkName);

        IEnumerable<IServerPark> GetServerParks();

        IEnumerable<string> GetNamesOfServerParks();

        bool ServerParkExists(string serverParkName);
    }
}
