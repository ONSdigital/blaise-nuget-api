using System.Collections.Generic;
using StatNeth.Blaise.API.ServerManager;

namespace Blaise.Nuget.Api.Contracts.Interfaces
{
    public interface IBlaiseUserApi
    {
        IEnumerable<IUser> GetUsers();

        IUser GetUser(string userName);

        bool UserExists(string userName);

        void AddUser(
            string userName,
            string password,
            string role,
            IList<string> serverParkNames,
            string defaultServerPark);

        void UpdatePassword(string userName, string password);

        void UpdateRole(string userName, string role);

        void UpdateServerParks(string userName, IEnumerable<string> serverParkNames, string defaultServerPark);

        void RemoveUser(string userName);

        bool ValidateUser(string userName, string password);
    }
}
