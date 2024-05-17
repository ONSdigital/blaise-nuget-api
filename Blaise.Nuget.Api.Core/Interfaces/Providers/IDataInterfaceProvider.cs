using StatNeth.Blaise.API.DataInterface;

namespace Blaise.Nuget.Api.Core.Interfaces.Providers
{
    public interface IDataInterfaceProvider
    {
        IDataInterface CreateFileDataInterface(string dataSourceFileName, string dataInterfaceFileName, string dataModelFileName);

        IDataInterface CreateSqlDataInterface(string databaseConnectionString, string dataInterfaceFileName, string dataModelFileName, bool createTables);

        IGeneralDataInterface CreateSettingsDataInterface(string databaseConnectionString, ApplicationType applicationType,
            string fileName);
    }
}