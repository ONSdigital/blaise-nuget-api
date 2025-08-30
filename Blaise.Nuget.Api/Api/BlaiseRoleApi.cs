namespace Blaise.Nuget.Api.Api
{
    using Blaise.Nuget.Api.Contracts.Interfaces;
    using Blaise.Nuget.Api.Contracts.Models;
    using Blaise.Nuget.Api.Core.Interfaces.Providers;
    using Blaise.Nuget.Api.Core.Interfaces.Services;
    using Blaise.Nuget.Api.Extensions;
    using Blaise.Nuget.Api.Providers;
    using StatNeth.Blaise.API.Security;
    using System.Collections.Generic;

    public class BlaiseRoleApi : IBlaiseRoleApi
    {
        private readonly IRoleService _roleService;

        private readonly ConnectionModel _connectionModel;

        public BlaiseRoleApi(
            IRoleService roleService,
            ConnectionModel connectionModel)
        {
            _roleService = roleService;
            _connectionModel = connectionModel;
        }

        public BlaiseRoleApi(ConnectionModel connectionModel = null)
        {
            _roleService = UnityProvider.Resolve<IRoleService>();

            var configurationProvider = UnityProvider.Resolve<IBlaiseConfigurationProvider>();
            _connectionModel = connectionModel ?? configurationProvider.GetConnectionModel();
        }

        public IEnumerable<IRole> GetRoles()
        {
            return _roleService.GetRoles(_connectionModel);
        }

        public IRole GetRole(string name)
        {
            name.ThrowExceptionIfNullOrEmpty("name");

            return _roleService.GetRole(_connectionModel, name);
        }

        public bool RoleExists(string name)
        {
            name.ThrowExceptionIfNullOrEmpty("name");

            return _roleService.RoleExists(_connectionModel, name);
        }

        public void AddRole(string name, string description, IEnumerable<string> permissions)
        {
            name.ThrowExceptionIfNullOrEmpty("name");

            _roleService.AddRole(_connectionModel, name, description, permissions);
        }

        public void RemoveRole(string name)
        {
            name.ThrowExceptionIfNullOrEmpty("name");

            _roleService.RemoveRole(_connectionModel, name);
        }

        public void UpdateRolePermissions(string name, IEnumerable<string> permissions)
        {
            name.ThrowExceptionIfNullOrEmpty("name");

            _roleService.UpdateRolePermissions(_connectionModel, name, permissions);
        }
    }
}
