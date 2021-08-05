using Blaise.Nuget.Api.Core.Factories;
using Blaise.Nuget.Api.Core.Interfaces.Factories;
using Blaise.Nuget.Api.Core.Interfaces.Mappers;
using Blaise.Nuget.Api.Core.Interfaces.Providers;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using Blaise.Nuget.Api.Core.Mappers;
using Blaise.Nuget.Api.Core.Providers;
using Blaise.Nuget.Api.Core.Services;
using Unity;

namespace Blaise.Nuget.Api.Providers
{
    public static class UnityProvider
    {
        private static readonly UnityContainer UnityContainer;

        static UnityProvider()
        {
            UnityContainer = new UnityContainer();

            // configuration provider
            UnityContainer.RegisterSingleton<IBlaiseConfigurationProvider, BlaiseConfigurationProvider>();

            //password service
            UnityContainer.RegisterType<IPasswordService, PasswordService>();

            //factories
            UnityContainer.RegisterType<IConnectedServerFactory, ConnectedServerFactory>();
            UnityContainer.RegisterType<IRemoteDataServerFactory, RemoteDataServerFactory>();
            UnityContainer.RegisterType<ICatiManagementServerFactory, CatiManagementServerFactory>();
            UnityContainer.RegisterType<ISecurityManagerFactory, SecurityManagerFactory>();
            UnityContainer.RegisterType<IDataInterfaceFactory, DataInterfaceFactory>();

            //mappers
            UnityContainer.RegisterType<IDataMapperService, DataMapperService>();
            UnityContainer.RegisterType<IRolePermissionMapper, RolePermissionMapper>();

            //data link providers
            UnityContainer.RegisterType<ILocalDataLinkProvider, LocalDataLinkProvider>();
            UnityContainer.RegisterType<IRemoteDataLinkProvider, RemoteDataLinkProvider>();
            UnityContainer.RegisterType<IDataInterfaceProvider, DataInterfaceProvider>();
            UnityContainer.RegisterType<IRemoteCatiManagementServerProvider, RemoteCatiManagementServerProvider>();

            //services
            UnityContainer.RegisterType<IDataModelService, DataModelService>();
            UnityContainer.RegisterType<IDataRecordService, DataRecordService>();
            UnityContainer.RegisterType<ICaseService, CaseService>();
            UnityContainer.RegisterType<IFieldService, FieldService>();
            UnityContainer.RegisterType<IKeyService, KeyService>();
            UnityContainer.RegisterType<IServerParkService, ServerParkService>();
            UnityContainer.RegisterType<ISurveyService, SurveyService>();
            UnityContainer.RegisterType<IUserService, UserService>();
            UnityContainer.RegisterType<IFileService, FileService>();
            UnityContainer.RegisterType<ICatiService, CatiService>();
            UnityContainer.RegisterType<IRoleService, RoleService>();
            UnityContainer.RegisterType<ISurveyMetaService, SurveyMetaService>();
            UnityContainer.RegisterType<ISqlService, SqlService>();
        }

        public static T Resolve<T>()
        {
            return UnityContainer.Resolve<T>();
        }
    }
}
