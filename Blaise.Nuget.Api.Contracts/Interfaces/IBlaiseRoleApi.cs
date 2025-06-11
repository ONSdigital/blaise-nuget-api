using System.Collections.Generic;
using StatNeth.Blaise.API.Security;

namespace Blaise.Nuget.Api.Contracts.Interfaces
{
    public interface IBlaiseRoleApi
    {
        IEnumerable<IRole> GetRoles();

        IRole GetRole(string name);

        bool RoleExists(string name);

        void AddRole(string name, string description, IEnumerable<string> permissions);

        void RemoveRole(string name);

        void UpdateRolePermissions(string name, IEnumerable<string> permissions);
    }
}
