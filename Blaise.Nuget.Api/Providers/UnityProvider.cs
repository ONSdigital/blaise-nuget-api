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
            UnityContainer.RegisterSingleton<IConnectedServerFactory, ConnectedServerFactory>();
            UnityContainer.RegisterSingleton<IRemoteDataServerFactory, RemoteDataServerFactory>();
            UnityContainer.RegisterType<ICatiManagementServerFactory, CatiManagementServerFactory>();
            UnityContainer.RegisterSingleton<ISecurityManagerFactory, SecurityManagerFactory>();
            UnityContainer.RegisterType<IDataInterfaceFactory, DataInterfaceFactory>();
            UnityContainer.RegisterType<IAuditTrailManagerFactory, AuditTrailManagerFactory>();


            //mappers
            UnityContainer.RegisterType<IDataRecordMapper, DataRecordMapper>();
            UnityContainer.RegisterType<IRolePermissionMapper, RolePermissionMapper>();
            UnityContainer.RegisterType<IAuditTrailDataMapper, AuditTrailDataMapper>();

            //data link providers
            UnityContainer.RegisterType<ILocalDataLinkProvider, LocalDataLinkProvider>();
            UnityContainer.RegisterSingleton<IRemoteDataLinkProvider, RemoteDataLinkProvider>();
            UnityContainer.RegisterType<IDataInterfaceProvider, DataInterfaceProvider>();
            UnityContainer.RegisterType<IRemoteCatiManagementServerProvider, RemoteCatiManagementServerProvider>();

            //services
            UnityContainer.RegisterType<IDataModelService, DataModelService>();
            UnityContainer.RegisterType<IDataRecordService, DataRecordService>();
            UnityContainer.RegisterType<ICaseService, CaseService>();
            UnityContainer.RegisterType<IFieldService, FieldService>();
            UnityContainer.RegisterType<IKeyService, KeyService>();
            UnityContainer.RegisterType<IServerParkService, ServerParkService>();
            UnityContainer.RegisterType<IQuestionnaireService, QuestionnaireService>();
            UnityContainer.RegisterType<IUserService, UserService>();
            UnityContainer.RegisterType<IFileService, FileService>();
            UnityContainer.RegisterType<ICatiService, CatiService>();
            UnityContainer.RegisterType<IRoleService, RoleService>();
            UnityContainer.RegisterType<IQuestionnaireMetaService, QuestionnaireMetaService>();
            UnityContainer.RegisterType<ISqlService, SqlService>();
            UnityContainer.RegisterType<IAuditTrailService, AuditTrailService>();
        }
        public static T Resolve<T>()
        {
            return UnityContainer.Resolve<T>();
        }
    }
}
