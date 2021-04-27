using System.IO;
using Blaise.Nuget.Api.Core.Interfaces.Factories;
using Blaise.Nuget.Api.Core.Interfaces.Providers;
using StatNeth.Blaise.API.DataInterface;


namespace Blaise.Nuget.Api.Core.Providers
{
    public class DataInterfaceProvider : IDataInterfaceProvider
    {
        private readonly IDataInterfaceFactory _dataInterfaceFactory;

        public DataInterfaceProvider(IDataInterfaceFactory dataInterfaceFactory)
        {
            _dataInterfaceFactory = dataInterfaceFactory;
        }

        public IDataInterface CreateFileDataInterface(string dataSourceFileName, string dataInterfaceFileName,
            string dataModelFileName)
        {
            var dataInterface = _dataInterfaceFactory.GetDataInterfaceForFile(dataSourceFileName);

            dataInterface.FileName = dataInterfaceFileName;
            dataInterface.DatamodelFileName = dataModelFileName;
            dataInterface.CreateTableDefinitions();
            dataInterface.CreateDatabaseObjects(null, true);

            //The full path is needed only to create table objects
            _dataInterfaceFactory.UpdateDataFileSource(dataInterface, Path.GetFileName(dataSourceFileName));
           
            dataInterface.SaveToFile(true);

            return dataInterface;
        }

        public IDataInterface CreateSqlDataInterface(string dataSourceFileName, string dataInterfaceFileName, string dataModelFileName)
        {
            var dataInterface = _dataInterfaceFactory.GetDataInterfaceForSql(dataSourceFileName);

            dataInterface.FileName = dataInterfaceFileName;
            dataInterface.DatamodelFileName = dataModelFileName;
            dataInterface.DataPartitionType = DataPartitionType.FlatBlocks;
            dataInterface.CreateTableDefinitions();
            dataInterface.CreateDatabaseObjects(dataInterface.ConnectionInfo.GetConnectionString(), true);
            dataInterface.SaveToFile(true);

            return dataInterface;
        }
    }
}
