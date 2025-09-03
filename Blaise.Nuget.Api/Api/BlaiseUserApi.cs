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

    public class BlaiseUserApi : IBlaiseUserApi
    {
        private readonly IUserService _userService;

        private readonly ConnectionModel _connectionModel;

        public BlaiseUserApi(
            IUserService userService,
            ConnectionModel connectionModel)
        {
            _userService = userService;
            _connectionModel = connectionModel;
        }

        public BlaiseUserApi(ConnectionModel connectionModel = null)
        {
            _userService = UnityProvider.Resolve<IUserService>();

            var configurationProvider = UnityProvider.Resolve<IBlaiseConfigurationProvider>();
            _connectionModel = connectionModel ?? configurationProvider.GetConnectionModel();
        }

        /// <inheritdoc/>
        public IEnumerable<IUser> GetUsers()
        {
            return _userService.GetUsers(_connectionModel);
        }

        /// <inheritdoc/>
        public IUser GetUser(string userName)
        {
            userName.ThrowExceptionIfNullOrEmpty("userName");

            return _userService.GetUser(_connectionModel, userName);
        }

        /// <inheritdoc/>
        public bool UserExists(string userName)
        {
            userName.ThrowExceptionIfNullOrEmpty("userName");

            return _userService.UserExists(_connectionModel, userName);
        }

        /// <inheritdoc/>
        public void AddUser(
            string userName,
            string password,
            string role,
            IList<string> serverParkNames,
            string defaultServerPark)
        {
            userName.ThrowExceptionIfNullOrEmpty("userName");
            password.ThrowExceptionIfNullOrEmpty("password");
            role.ThrowExceptionIfNullOrEmpty("role");
            defaultServerPark.ThrowExceptionIfNullOrEmpty("DefaultServerPark");

            _userService.AddUser(_connectionModel, userName, password, role, serverParkNames, defaultServerPark);
        }

        /// <inheritdoc/>
        public void UpdatePassword(string userName, string password)
        {
            userName.ThrowExceptionIfNullOrEmpty("userName");
            password.ThrowExceptionIfNullOrEmpty("password");

            _userService.UpdatePassword(_connectionModel, userName, password);
        }

        /// <inheritdoc/>
        public void UpdateRole(string userName, string role)
        {
            userName.ThrowExceptionIfNullOrEmpty("userName");
            role.ThrowExceptionIfNullOrEmpty("role");

            _userService.UpdateRole(_connectionModel, userName, role);
        }

        /// <inheritdoc/>
        public void UpdateServerParks(
            string userName,
            IEnumerable<string> serverParkNames,
            string defaultServerPark)
        {
            userName.ThrowExceptionIfNullOrEmpty("userName");
            defaultServerPark.ThrowExceptionIfNullOrEmpty("defaultServerPark");

            _userService.UpdateServerParks(_connectionModel, userName, serverParkNames, defaultServerPark);
        }

        /// <inheritdoc/>
        public void RemoveUser(string userName)
        {
            userName.ThrowExceptionIfNullOrEmpty("userName");

            _userService.RemoveUser(_connectionModel, userName);
        }

        /// <inheritdoc/>
        public bool ValidateUser(string userName, string password)
        {
            userName.ThrowExceptionIfNullOrEmpty("userName");
            password.ThrowExceptionIfNullOrEmpty("password");

            return _userService.ValidateUser(_connectionModel, userName, password);
        }
    }
}
