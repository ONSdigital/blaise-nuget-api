namespace Blaise.Nuget.Api.Core.Interfaces.Factories
{
    using StatNeth.Blaise.API.DataInterface;

    public interface IDataInterfaceFactory
    {
        IDataInterface GetDataInterfaceForFile(string dataSourceFileName);

        void UpdateDataFileSource(IDataInterface dataInterface, string dataSourceFileName);

        IDataInterface GetDataInterfaceForSql(string databaseConnectionString);

        IGeneralDataInterface GetSettingsDataInterfaceForSql(
            string databaseConnectionString,
            ApplicationType applicationType);
    }
}
