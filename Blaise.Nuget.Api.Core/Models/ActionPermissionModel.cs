using StatNeth.Blaise.API.Security;

namespace Blaise.Nuget.Api.Core.Models
{
    public class ActionPermissionModel : IActionPermission
    {
        public string Action { get; set; }

        public PermissionStatus Permission { get; set; }
    }
}
