namespace Blaise.Nuget.Api.Core.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Blaise.Nuget.Api.Contracts.Exceptions;
    using Blaise.Nuget.Api.Contracts.Models;
    using Blaise.Nuget.Api.Core.Interfaces.Factories;
    using Blaise.Nuget.Api.Core.Interfaces.Mappers;
    using Blaise.Nuget.Api.Core.Interfaces.Services;
    using StatNeth.Blaise.API.Security;

    public class RoleService : IRoleService
    {
        private readonly ISecurityManagerFactory _securityManagerFactory;

        private readonly IRolePermissionMapper _mapper;

        public RoleService(
            ISecurityManagerFactory securityManagerFactory,
            IRolePermissionMapper mapper)
        {
            _securityManagerFactory = securityManagerFactory;
            _mapper = mapper;
        }

        public IEnumerable<IRole> GetRoles(ConnectionModel connectionModel)
        {
            var connection = _securityManagerFactory.GetConnection(connectionModel);

            return connection.GetRoles();
        }

        public IRole GetRole(ConnectionModel connectionModel, string name)
        {
            var roles = GetRoles(connectionModel);
            var role = roles.FirstOrDefault(r => r.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

            if (role == null)
            {
                throw new DataNotFoundException($"The role '{name}' was not found");
            }

            return role;
        }

        public bool RoleExists(ConnectionModel connectionModel, string name)
        {
            var roles = GetRoles(connectionModel);

            return roles.Any(r => r.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public void AddRole(ConnectionModel connectionModel, string name, string description, IEnumerable<string> permissions)
        {
            var connection = _securityManagerFactory.GetConnection(connectionModel);
            var roleId = connection.AddRole(name, description);

            var actionPermissions = _mapper.MapToActionPermissionModels(permissions);

            connection.UpdateRolePermissions(roleId, actionPermissions);
        }

        public void RemoveRole(ConnectionModel connectionModel, string name)
        {
            var role = GetRole(connectionModel, name);
            var connection = _securityManagerFactory.GetConnection(connectionModel);

            connection.RemoveRole(role.Id);
        }

        public void UpdateRolePermissions(ConnectionModel connectionModel, string name, IEnumerable<string> permissions)
        {
            var role = GetRole(connectionModel, name);
            var actionPermissions = _mapper.MapToActionPermissionModels(permissions);

            var connection = _securityManagerFactory.GetConnection(connectionModel);

            connection.UpdateRolePermissions(role.Id, actionPermissions);
        }
    }
}
