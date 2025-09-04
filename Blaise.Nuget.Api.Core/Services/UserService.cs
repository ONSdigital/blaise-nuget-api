namespace Blaise.Nuget.Api.Core.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Blaise.Nuget.Api.Contracts.Models;
    using Blaise.Nuget.Api.Core.Interfaces.Factories;
    using Blaise.Nuget.Api.Core.Interfaces.Services;
    using StatNeth.Blaise.API.ServerManager;

    public class UserService : IUserService
    {
        private readonly IConnectedServerFactory _connectedServerFactory;

        private readonly IPasswordService _passwordService;

        public UserService(
            IConnectedServerFactory connectedServerFactory,
            IPasswordService passwordService)
        {
            _connectedServerFactory = connectedServerFactory;
            _passwordService = passwordService;
        }

        /// <inheritdoc/>
        public IEnumerable<IUser> GetUsers(ConnectionModel connectionModel)
        {
            var connection = _connectedServerFactory.GetConnection(connectionModel);

            return connection.Users;
        }

        /// <inheritdoc/>
        public IUser GetUser(ConnectionModel connectionModel, string userName)
        {
            var connection = _connectedServerFactory.GetConnection(connectionModel);

            return connection.Users.GetItem(userName);
        }

        /// <inheritdoc/>
        public bool UserExists(ConnectionModel connectionModel, string userName)
        {
            var connection = _connectedServerFactory.GetConnection(connectionModel);

            return connection.Users.Any(u => u.Name.Equals(userName, StringComparison.OrdinalIgnoreCase));
        }

        /// <inheritdoc/>
        public void AddUser(
            ConnectionModel connectionModel,
            string userName,
            string password,
            string role,
            IEnumerable<string> serverParkNames,
            string defaultServerPark)
        {
            var connection = _connectedServerFactory.GetConnection(connectionModel);
            var securePassword = _passwordService.CreateSecurePassword(password);
            var user = (IUser2)connection.AddUser(userName, securePassword);

            AssignRoleToUser(user, role);
            AddServerParksToUser(user, serverParkNames);
            AddCatiPreferenceToUser(user, defaultServerPark);

            user.Save();
        }

        /// <inheritdoc/>
        public void UpdatePassword(ConnectionModel connectionModel, string userName, string password)
        {
            var securePassword = _passwordService.CreateSecurePassword(password);
            var user = (IUser2)GetUser(connectionModel, userName);

            user.ChangePassword(securePassword);
            user.Save();
        }

        /// <inheritdoc/>
        public void UpdateRole(ConnectionModel connectionModel, string userName, string role)
        {
            var user = (IUser2)GetUser(connectionModel, userName);

            AssignRoleToUser(user, role);
            user.Save();
        }

        /// <inheritdoc/>
        public void UpdateServerParks(
            ConnectionModel connectionModel,
            string userName,
            IEnumerable<string> serverParkNames,
            string defaultServerPark)
        {
            var user = (IUser2)GetUser(connectionModel, userName);

            user.ServerParks.Clear();
            AddServerParksToUser(user, serverParkNames);
            AddCatiPreferenceToUser(user, defaultServerPark);
            user.Save();
        }

        /// <inheritdoc/>
        public void RemoveUser(ConnectionModel connectionModel, string userName)
        {
            var connection = _connectedServerFactory.GetConnection(connectionModel);

            connection.RemoveUser(userName);
        }

        /// <inheritdoc/>
        public bool ValidateUser(ConnectionModel connectionModel, string userName, string password)
        {
            try
            {
                _connectedServerFactory.GetIsolatedConnection(new ConnectionModel
                {
                    ServerName = connectionModel.ServerName,
                    UserName = userName,
                    Password = password,
                    Binding = connectionModel.Binding,
                    Port = connectionModel.Port,
                    ConnectionExpiresInMinutes = connectionModel.ConnectionExpiresInMinutes,
                });
                return true;
            }
            catch
            {
                // currently only way to validate user...
                return false;
            }
        }

        private static void AddServerParksToUser(IUser user, IEnumerable<string> serverParkNames)
        {
            foreach (var serverParkName in serverParkNames)
            {
                user.ServerParks.Add(serverParkName);
            }
        }

        private static void AssignRoleToUser(IUser2 user, string role)
        {
            try
            {
                user.Role = role;
            }
            catch
            {
                // still continue..!?
            }
        }

        private static void AddCatiPreferenceToUser(IUser2 user, string defaultServerPark)
        {
            if (user.Preferences.All(userPreference => userPreference.Type != "CATI.Preferences"))
            {
                user.Preferences.Add("CATI.Preferences");
            }

            var catiPreference = user.Preferences.GetItem("CATI.Preferences");
            catiPreference.Value = $"<CatiDashboard><ServerPark>{defaultServerPark}</ServerPark></CatiDashboard>";
        }
    }
}
