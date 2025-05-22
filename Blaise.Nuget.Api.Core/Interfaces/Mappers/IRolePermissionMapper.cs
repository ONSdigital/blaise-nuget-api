using System.Collections.Generic;
using Blaise.Nuget.Api.Core.Models;

namespace Blaise.Nuget.Api.Core.Interfaces.Mappers
{
    public interface IRolePermissionMapper
    {
        IEnumerable<ActionPermissionModel> MapToActionPermissionModels(IEnumerable<string> permissions);
    }
}
