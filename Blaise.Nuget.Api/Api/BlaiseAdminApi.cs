using System.Collections.Generic;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Admin;
using Blaise.Nuget.Api.Core.Interfaces.Factories;
using Blaise.Nuget.Api.Core.Interfaces.Providers;
using Blaise.Nuget.Api.Providers;

namespace Blaise.Nuget.Api.Api
{
    public class BlaiseAdminApi : IBlaiseAdminApi
    {
        public void ResetConnections()
        {
            ResetConnection<ICatiManagementServerFactory>();
            ResetConnection<IConnectedServerFactory>();
            ResetConnection<IRemoteDataServerFactory>();
            ResetConnection<ISecurityManagerFactory>();

            ResetConnection<IRemoteDataLinkProvider>();
            ResetConnection<ILocalDataLinkProvider>();
        }

        public IEnumerable<OpenConnectionModel> OpenConnections()
        {
            var openConnections = new List<OpenConnectionModel>
            {
                GetOpenConnectionDetails<ICatiManagementServerFactory>(),
                GetOpenConnectionDetails<IConnectedServerFactory>(),
                GetOpenConnectionDetails<IRemoteDataServerFactory>(),
                GetOpenConnectionDetails<ISecurityManagerFactory>(),

                GetOpenConnectionDetails<IRemoteDataLinkProvider>(),
                GetOpenConnectionDetails<ILocalDataLinkProvider>()
            };

            return openConnections;
        }

        public void ResetConnection<T>() where T : IResetConnections
        {
            var service = UnityProvider.Resolve<T>();
            service.ResetConnections();
        }

        public OpenConnectionModel GetOpenConnectionDetails<T>() where T : IGetOpenConnections
        {
            var service = UnityProvider.Resolve<T>();
            return new OpenConnectionModel
            {
                ConnectionType = typeof(T).FullName,
                NumberOfConnections = service.GetNumberOfOpenConnections(),
                Connections = service.GetConnections()
            };
        }
    }
}
