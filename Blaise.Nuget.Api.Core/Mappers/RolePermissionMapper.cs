namespace Blaise.Nuget.Api.Core.Mappers
{
    using Blaise.Nuget.Api.Core.Interfaces.Mappers;
    using Blaise.Nuget.Api.Core.Models;
    using StatNeth.Blaise.API.Security;
    using System.Collections.Generic;

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
