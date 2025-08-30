namespace Blaise.Nuget.Api.Core.Interfaces.Services
{
    using Blaise.Nuget.Api.Contracts.Models;
    using StatNeth.Blaise.API.Security;
    using System.Collections.Generic;

    public interface IRoleService
    {
        IEnumerable<IRole> GetRoles(ConnectionModel connectionModel);

        IRole GetRole(ConnectionModel connectionModel, string name);

        bool RoleExists(ConnectionModel connectionModel, string name);

        void AddRole(ConnectionModel connectionModel, string name, string description, IEnumerable<string> permissions);

        void RemoveRole(ConnectionModel connectionModel, string name);

        void UpdateRolePermissions(ConnectionModel connectionModel, string name, IEnumerable<string> permissions);
    }
}
