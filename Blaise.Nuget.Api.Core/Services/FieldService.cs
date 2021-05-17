using System.Linq;
using Blaise.Nuget.Api.Contracts.Enums;
using Blaise.Nuget.Api.Contracts.Extensions;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using StatNeth.Blaise.API.DataRecord;
using StatNeth.Blaise.API.Meta;

namespace Blaise.Nuget.Api.Core.Services
{
    public class FieldService : IFieldService
    {
        private readonly IDataModelService _dataModelService;

        public FieldService(IDataModelService dataModelService)
        {
            _dataModelService = dataModelService;
        }

        public bool FieldExists(ConnectionModel connectionModel, string instrumentName, string serverParkName, FieldNameType fieldNameType)
        {
            var dataModel = _dataModelService.GetDataModel(connectionModel, instrumentName, serverParkName);
            var definitionScope = (IDefinitionScope2)dataModel;

            return definitionScope.FieldExists(fieldNameType.FullName());
        }

        public bool FieldExists(IDataRecord dataRecord, FieldNameType fieldNameType)
        {
            var dataRecord2 = (IDataRecord2)dataRecord;

            return dataRecord2.Fields.Any(f => f.FullName == fieldNameType.FullName());
        }

        public IField GetField(IDataRecord dataRecord, FieldNameType fieldNameType)
        {
            return dataRecord.GetField(fieldNameType.FullName());
        }
    }
}
