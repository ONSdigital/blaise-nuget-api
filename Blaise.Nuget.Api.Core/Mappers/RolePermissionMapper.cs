using System.Collections.Generic;
using Blaise.Nuget.Api.Core.Interfaces.Mappers;
using Blaise.Nuget.Api.Core.Models;
using StatNeth.Blaise.API.Security;

namespace Blaise.Nuget.Api.Core.Mappers
{
    public class RolePermissionMapper : IRolePermissionMapper
    {
        public IEnumerable<ActionPermissionModel> MapToActionPermissionModels(IEnumerable<string> permissions)
        {
            var actionPermissions = new List<ActionPermissionModel>();

            foreach (var permission in permissions)
            {
                actionPermissions.Add(new ActionPermissionModel
                {
                    Action = permission,
                    Permission = PermissionStatus.Allowed
                });
            }

            return actionPermissions;
        }
    }
}
