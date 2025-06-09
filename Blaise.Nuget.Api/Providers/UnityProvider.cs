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
        private static readonly UnityContainer _UnityContainer;

        static UnityProvider()
        {
            _UnityContainer = new UnityContainer();

            // configuration provider
            _UnityContainer.RegisterSingleton<IBlaiseConfigurationProvider, BlaiseConfigurationProvider>();

            // password service
            _UnityContainer.RegisterType<IPasswordService, PasswordService>();

            // factories
            _UnityContainer.RegisterSingleton<IConnectedServerFactory, ConnectedServerFactory>();
            _UnityContainer.RegisterSingleton<IRemoteDataServerFactory, RemoteDataServerFactory>();
            _UnityContainer.RegisterType<ICatiManagementServerFactory, CatiManagementServerFactory>();
            _UnityContainer.RegisterSingleton<ISecurityManagerFactory, SecurityManagerFactory>();
            _UnityContainer.RegisterType<IDataInterfaceFactory, DataInterfaceFactory>();
            _UnityContainer.RegisterType<IAuditTrailManagerFactory, AuditTrailManagerFactory>();

            // mappers
            _UnityContainer.RegisterType<IDataRecordMapper, DataRecordMapper>();
            _UnityContainer.RegisterType<IRolePermissionMapper, RolePermissionMapper>();
            _UnityContainer.RegisterType<IAuditTrailDataMapper, AuditTrailDataMapper>();

            // data link providers
            _UnityContainer.RegisterType<ILocalDataLinkProvider, LocalDataLinkProvider>();
            _UnityContainer.RegisterSingleton<IRemoteDataLinkProvider, RemoteDataLinkProvider>();
            _UnityContainer.RegisterType<IDataInterfaceProvider, DataInterfaceProvider>();
            _UnityContainer.RegisterType<IRemoteCatiManagementServerProvider, RemoteCatiManagementServerProvider>();

            // services
            _UnityContainer.RegisterType<IDataModelService, DataModelService>();
            _UnityContainer.RegisterType<IDataRecordService, DataRecordService>();
            _UnityContainer.RegisterType<ICaseService, CaseService>();
            _UnityContainer.RegisterType<IFieldService, FieldService>();
            _UnityContainer.RegisterType<IKeyService, KeyService>();
            _UnityContainer.RegisterType<IServerParkService, ServerParkService>();
            _UnityContainer.RegisterType<IQuestionnaireService, QuestionnaireService>();
            _UnityContainer.RegisterType<IUserService, UserService>();
            _UnityContainer.RegisterType<IFileService, FileService>();
            _UnityContainer.RegisterType<ICatiService, CatiService>();
            _UnityContainer.RegisterType<IRoleService, RoleService>();
            _UnityContainer.RegisterType<IQuestionnaireMetaService, QuestionnaireMetaService>();
            _UnityContainer.RegisterType<ISqlService, SqlService>();
            _UnityContainer.RegisterType<IAuditTrailService, AuditTrailService>();
        }

        public static T Resolve<T>()
        {
            return _UnityContainer.Resolve<T>();
        }
    }
}
