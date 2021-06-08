using System.Collections.Generic;
using Blaise.Nuget.Api.Contracts.Models;

namespace Blaise.Nuget.Api.Contracts.Interfaces
{
    public interface IBlaiseAdminApi
    {
        void ResetConnections();

        IEnumerable<OpenConnectionModel> OpenConnections();
    }
}