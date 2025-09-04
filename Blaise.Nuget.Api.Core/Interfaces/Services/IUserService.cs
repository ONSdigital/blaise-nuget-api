namespace Blaise.Nuget.Api.Core.Interfaces.Services
{
    using System.Collections.Generic;
    using Blaise.Nuget.Api.Contracts.Models;
    using StatNeth.Blaise.API.ServerManager;

    public interface IUserService
    {
        IEnumerable<IUser> GetUsers(ConnectionModel connectionModel);

        IUser GetUser(ConnectionModel connectionModel, string userName);

        bool UserExists(ConnectionModel connectionModel, string userName);

        void AddUser(ConnectionModel connectionModel, string userName, string password, string role, IEnumerable<string> serverParkNames, string defaultServerPark);

        void UpdatePassword(ConnectionModel connectionModel, string userName, string password);

        void UpdateRole(ConnectionModel connectionModel, string userName, string role);

        void UpdateServerParks(ConnectionModel connectionModel, string userName, IEnumerable<string> serverParkNames, string defaultServerPark);

        void RemoveUser(ConnectionModel connectionModel, string userName);

        bool ValidateUser(ConnectionModel connectionModel, string userName, string password);
    }
}
