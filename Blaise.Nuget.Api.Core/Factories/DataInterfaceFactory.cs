namespace Blaise.Nuget.Api.Core.Factories
{
    using Blaise.Nuget.Api.Core.Interfaces.Factories;
    using StatNeth.Blaise.API.DataInterface;

    public class DataInterfaceFactory : IDataInterfaceFactory
    {
        public IDataInterface GetDataInterfaceForFile(string dataSourceFileName)
        {
            var dataInterface = DataInterfaceManager.GetDataInterface();
            dataInterface.ConnectionInfo.DataSourceType = DataSourceType.Blaise;
            dataInterface.ConnectionInfo.DataProviderType = DataProviderType.BlaiseDataProviderForDotNET;
            dataInterface.DataPartitionType = DataPartitionType.Stream;

            var connectionBuilder = DataInterfaceManager.GetBlaiseConnectionStringBuilder();
            connectionBuilder.DataSource = dataSourceFileName;
            dataInterface.ConnectionInfo.SetConnectionString(connectionBuilder.ConnectionString);

            return dataInterface;
        }

        public void UpdateDataFileSource(IDataInterface dataInterface, string dataSourceFileName)
        {
            var connectionBuilder = DataInterfaceManager.GetBlaiseConnectionStringBuilder();
            connectionBuilder.DataSource = dataSourceFileName;
            dataInterface.ConnectionInfo.SetConnectionString(connectionBuilder.ConnectionString);
        }

        public IDataInterface GetDataInterfaceForSql(string databaseConnectionString)
        {
            var dataInterface = DataInterfaceManager.GetDataInterface();
            dataInterface.ConnectionInfo.DataSourceType = DataSourceType.MySQL;
            dataInterface.ConnectionInfo.DataProviderType = DataProviderType.MySQLDataProvider;
            dataInterface.DataPartitionType = DataPartitionType.Stream;

            var connectionStringBuilder = DataInterfaceManager.GetBlaiseConnectionStringBuilder();
            connectionStringBuilder.ConnectionString = databaseConnectionString;
            dataInterface.ConnectionInfo.SetConnectionString(connectionStringBuilder.ConnectionString);

            return dataInterface;
        }

        public IGeneralDataInterface GetSettingsDataInterfaceForSql(
            string databaseConnectionString,
            ApplicationType applicationType)
        {
            var dataInterface = DataInterfaceManager.Create(applicationType);
            dataInterface.ConnectionInfo.DataSourceType = DataSourceType.MySQL;
            dataInterface.ConnectionInfo.DataProviderType = DataProviderType.MySQLDataProvider;

            var connectionBuilder = DataInterfaceManager.GetBlaiseConnectionStringBuilder();
            connectionBuilder.ConnectionString = databaseConnectionString;
            dataInterface.ConnectionInfo.SetConnectionString(connectionBuilder.ConnectionString);

            return dataInterface;
        }
    }
}
