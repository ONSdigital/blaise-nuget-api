using System.Collections.Generic;
using Blaise.Nuget.Api.Core.Interfaces.Mappers;
using StatNeth.Blaise.API.DataRecord;
using StatNeth.Blaise.API.Meta;

namespace Blaise.Nuget.Api.Core.Mappers
{
    public class DataRecordMapper : IDataRecordMapper
    {
        public IDataRecord MapDataRecordFields(IDataRecord dataRecord, IKey key,
            string primaryKeyValue, Dictionary<string, string> fieldData)
        {
            var idField = dataRecord.GetField(key.Fields[0].FullName);
            idField.DataValue.Assign(primaryKeyValue);

            return MapDataRecordFields(dataRecord, fieldData);
        }

        public IDataRecord MapDataRecordFields(IDataRecord dataRecord, Dictionary<string, string> fieldData)
        {
            var definitionScope = (IDefinitionScope2)dataRecord.Datamodel;

            foreach (var field in fieldData)
            {
                /* Adding try / catch around processing payload fields so that it doesn't stop
                   if a field is found that isn't in the Blaise data model as we still want to process the 
                   remaining fields */
                try
                {
                    if (!definitionScope.FieldExists(field.Key))
                    {
                        continue;
                    }

                    var item = dataRecord.GetField(field.Key);
                    item.DataValue.Assign(field.Value);
                }
                // ReSharper disable once EmptyGeneralCatchClause
                catch
                {
                }
            }

            return dataRecord;
        }

        public Dictionary<string, string> MapFieldDictionaryFromRecord(IDataRecord dataRecord)
        {
            var fieldDictionary = new Dictionary<string, string>();
            var dataRecord2 = (IDataRecord2) dataRecord;
            var dataFields = dataRecord2.GetDataFields();

            foreach (var dataField in dataFields)
            {
                fieldDictionary[dataField.FullName] = dataField.DataValue.ValueAsText;
            }

            return fieldDictionary;
        }
    }
}