namespace Blaise.Nuget.Api.Core.Interfaces.Providers
{
    using StatNeth.Blaise.API.DataInterface;

    public interface IDataInterfaceProvider
    {
        IDataInterface CreateFileDataInterface(
            string dataSourceFileName,
            string dataInterfaceFileName,
            string dataModelFileName);

        IDataInterface CreateSqlDataInterface(
            string databaseConnectionString,
            string dataInterfaceFileName,
            string dataModelFileName,
            bool createDatabaseObjects);

        IGeneralDataInterface CreateSettingsDataInterface(
            string databaseConnectionString,
            ApplicationType applicationType,
            string fileName);
    }
}
