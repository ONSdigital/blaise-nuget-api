namespace Blaise.Nuget.Api.Providers
{
    using Blaise.Nuget.Api.Core.Factories;
    using Blaise.Nuget.Api.Core.Interfaces.Factories;
    using Blaise.Nuget.Api.Core.Interfaces.Mappers;
    using Blaise.Nuget.Api.Core.Interfaces.Providers;
    using Blaise.Nuget.Api.Core.Interfaces.Services;
    using Blaise.Nuget.Api.Core.Mappers;
    using Blaise.Nuget.Api.Core.Providers;
    using Blaise.Nuget.Api.Core.Services;
    using Unity;

    public static class UnityProvider
    {
        private static readonly UnityContainer _unityContainer;

        static UnityProvider()
        {
            _unityContainer = new UnityContainer();

            // factories
            _unityContainer.RegisterSingleton<IConnectedServerFactory, ConnectedServerFactory>();
            _unityContainer.RegisterSingleton<IRemoteDataServerFactory, RemoteDataServerFactory>();
            _unityContainer.RegisterType<ICatiManagementServerFactory, CatiManagementServerFactory>();
            _unityContainer.RegisterSingleton<ISecurityManagerFactory, SecurityManagerFactory>();
            _unityContainer.RegisterType<IDataInterfaceFactory, DataInterfaceFactory>();
            _unityContainer.RegisterType<IAuditTrailManagerFactory, AuditTrailManagerFactory>();

            // mappers
            _unityContainer.RegisterType<IDataRecordMapper, DataRecordMapper>();
            _unityContainer.RegisterType<IRolePermissionMapper, RolePermissionMapper>();
            _unityContainer.RegisterType<IAuditTrailDataMapper, AuditTrailDataMapper>();

            // providers
            _unityContainer.RegisterSingleton<IBlaiseConfigurationProvider, BlaiseConfigurationProvider>();
            _unityContainer.RegisterType<ILocalDataLinkProvider, LocalDataLinkProvider>();
            _unityContainer.RegisterSingleton<IRemoteDataLinkProvider, RemoteDataLinkProvider>();
            _unityContainer.RegisterType<IDataInterfaceProvider, DataInterfaceProvider>();
            _unityContainer.RegisterType<IRemoteCatiManagementServerProvider, RemoteCatiManagementServerProvider>();

            // services
            _unityContainer.RegisterType<IDataModelService, DataModelService>();
            _unityContainer.RegisterType<IDataRecordService, DataRecordService>();
            _unityContainer.RegisterType<ICaseService, CaseService>();
            _unityContainer.RegisterType<IFieldService, FieldService>();
            _unityContainer.RegisterType<IKeyService, KeyService>();
            _unityContainer.RegisterType<IServerParkService, ServerParkService>();
            _unityContainer.RegisterType<IQuestionnaireService, QuestionnaireService>();
            _unityContainer.RegisterType<IUserService, UserService>();
            _unityContainer.RegisterType<IFileService, FileService>();
            _unityContainer.RegisterType<ICatiService, CatiService>();
            _unityContainer.RegisterType<IRoleService, RoleService>();
            _unityContainer.RegisterType<IQuestionnaireMetaService, QuestionnaireMetaService>();
            _unityContainer.RegisterType<ISqlService, SqlService>();
            _unityContainer.RegisterType<IAuditTrailService, AuditTrailService>();
            _unityContainer.RegisterType<IPasswordService, PasswordService>();
        }

        public static T Resolve<T>()
        {
            return _unityContainer.Resolve<T>();
        }
    }
}
