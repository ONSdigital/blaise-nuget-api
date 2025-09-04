namespace Blaise.Nuget.Api.Core.Models
{
    using StatNeth.Blaise.API.Security;

    public class ActionPermissionModel : IActionPermission
    {
        /// <inheritdoc/>
        public string Action { get; set; }

        /// <inheritdoc/>
        public PermissionStatus Permission { get; set; }
    }
}
