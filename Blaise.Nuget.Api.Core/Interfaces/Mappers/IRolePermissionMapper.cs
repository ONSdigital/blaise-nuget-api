namespace Blaise.Nuget.Api.Core.Interfaces.Mappers
{
    using Blaise.Nuget.Api.Core.Models;
    using System.Collections.Generic;

    public interface IRolePermissionMapper
    {
        IEnumerable<ActionPermissionModel> MapToActionPermissionModels(IEnumerable<string> permissions);
    }
}
