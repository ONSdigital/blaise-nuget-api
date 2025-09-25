namespace Blaise.Nuget.Api.Core.Mappers
{
    using System;
    using System.Collections.Generic;
    using Blaise.Nuget.Api.Core.Interfaces.Mappers;
    using StatNeth.Blaise.API.DataRecord;
    using StatNeth.Blaise.API.Meta;

    public class DataRecordMapper : IDataRecordMapper
    {
        public IDataRecord MapDataRecordFields(
            IDataRecord dataRecord,
            IKey key,
            Dictionary<string, string> primaryKeyValues,
            Dictionary<string, string> fieldData)
        {
            foreach (var item in primaryKeyValues)
            {
                var idField = dataRecord.GetField(item.Key);
                idField.DataValue.Assign(item.Value);
            }

            return MapDataRecordFields(dataRecord, fieldData);
        }

        public IDataRecord MapDataRecordFields(IDataRecord dataRecord, Dictionary<string, string> fieldData)
        {
            var definitionScope = (IDefinitionScope2)dataRecord.Datamodel;

            foreach (var field in fieldData)
            {
                // adding try/catch when processing payload fields ensures the process continues
                // even if a field doesn't match the Blaise data model we still want the process to continue
                try
                {
                    if (!definitionScope.FieldExists(field.Key))
                    {
                        continue;
                    }

                    var item = dataRecord.GetField(field.Key);
                    item.DataValue.Assign(field.Value);
                }
                catch (Exception ex)
                {
                    // a decision has been made to not fail the entire process if a field fails to map
                    // we are logging the error to ensure it is not hidden
                    Console.WriteLine($"Failed to map field '{field.Key}' with value '{field.Value}': {exception}");
                }
            }

            return dataRecord;
        }

        public Dictionary<string, string> MapFieldDictionaryFromRecord(IDataRecord dataRecord)
        {
            var fieldDictionary = new Dictionary<string, string>();
            var dataRecord2 = (IDataRecord2)dataRecord;
            var dataFields = dataRecord2.GetDataFields();

            foreach (var dataField in dataFields)
            {
                fieldDictionary[dataField.FullName] = dataField.DataValue.ValueAsText;
            }

            return fieldDictionary;
        }
    }
}
