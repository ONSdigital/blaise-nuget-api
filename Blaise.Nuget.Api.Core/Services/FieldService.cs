namespace Blaise.Nuget.Api.Core.Services
{
    using Blaise.Nuget.Api.Contracts.Models;
    using Blaise.Nuget.Api.Core.Interfaces.Services;
    using StatNeth.Blaise.API.DataRecord;
    using StatNeth.Blaise.API.Meta;

    public class FieldService : IFieldService
    {
        private readonly IDataModelService _dataModelService;

        public FieldService(IDataModelService dataModelService)
        {
            _dataModelService = dataModelService;
        }

        /// <inheritdoc/>
        public bool FieldExists(ConnectionModel connectionModel, string questionnaireName, string serverParkName, string fieldName)
        {
            var dataModel = _dataModelService.GetDataModel(connectionModel, questionnaireName, serverParkName);

            return FieldExists(dataModel, fieldName);
        }

        /// <inheritdoc/>
        public bool FieldExists(IDataRecord dataRecord, string fieldName)
        {
            return FieldExists(dataRecord.Datamodel, fieldName);
        }

        /// <inheritdoc/>
        public IField GetField(IDataRecord dataRecord, string fieldName)
        {
            return dataRecord.GetField(fieldName);
        }

        private bool FieldExists(IDatamodel dataModel, string fieldName)
        {
            var definitionScope = (IDefinitionScope2)dataModel;

            return definitionScope.FieldExists(fieldName);
        }
    }
}
