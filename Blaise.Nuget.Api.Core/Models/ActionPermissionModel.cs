using StatNeth.Blaise.API.Security;

namespace Blaise.Nuget.Api.Core.Models
{
    public class ActionPermissionModel : IActionPermission
    {
        /// <inheritdoc/>
        public string Action { get; set; }

        /// <inheritdoc/>
        public PermissionStatus Permission { get; set; }
    }
}
