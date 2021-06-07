using Blaise.Nuget.Api.Contracts.Interfaces;
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

        public int ActiveConnections()
        {
            return CurrentConnections<IRemoteDataLinkProvider>();
        }

        public void ResetConnection<T>() where T : IResetConnections
        {
            var service = UnityProvider.Resolve<T>();
            service.ResetConnections();
        }

        public int CurrentConnections<T>() where T : IGetActiveConnections
        {
            var service = UnityProvider.Resolve<T>();
            return service.NumberOfConnections();
        }
    }
}
