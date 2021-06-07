using Blaise.Nuget.Api.Contracts.Interfaces;
using Blaise.Nuget.Api.Core.Interfaces.Admin;
using Blaise.Nuget.Api.Core.Interfaces.Factories;
using Blaise.Nuget.Api.Core.Interfaces.Providers;
using Blaise.Nuget.Api.Interfaces;
using Blaise.Nuget.Api.Providers;

namespace Blaise.Nuget.Api.Api
{
    public class BlaiseAdminApi : IBlaiseAdminApi
    {
        private readonly IIocProvider _iocProvider;

        internal BlaiseAdminApi(IIocProvider iocProvider)
        {
            _iocProvider = iocProvider;
        }

        public BlaiseAdminApi()
        {
            _iocProvider = new UnityProvider();
            _iocProvider.RegisterDependencies();
        }

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
            return CurrentConnections<IRemoteDataServerFactory>();
        }

        public void ResetConnection<T>() where T : IResetConnections
        {
            var service = _iocProvider.Resolve<T>();
            service.ResetConnections();
        }

        public int CurrentConnections<T>() where T : IGetActiveConnections
        {
            var service = _iocProvider.Resolve<T>();
            return service.NumberOfConnections();
        }
    }
}
