namespace Blaise.Nuget.Api.Core.Models
{
    using StatNeth.Blaise.API.Security;

    public class ActionPermissionModel : IActionPermission
    {
        public string Action { get; set; }

        public PermissionStatus Permission { get; set; }
    }
}
