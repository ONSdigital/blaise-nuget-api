namespace Blaise.Nuget.Api.Core.Interfaces.Mappers
{
    using System.Collections.Generic;
    using Blaise.Nuget.Api.Core.Models;

    public interface IRolePermissionMapper
    {
        IEnumerable<ActionPermissionModel> MapToActionPermissionModels(IEnumerable<string> permissions);
    }
}
