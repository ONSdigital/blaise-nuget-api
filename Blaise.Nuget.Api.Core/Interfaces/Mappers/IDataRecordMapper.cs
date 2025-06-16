using System.Collections.Generic;
using StatNeth.Blaise.API.DataRecord;

namespace Blaise.Nuget.Api.Core.Interfaces.Mappers
{
    public interface IDataRecordMapper
    {
        IDataRecord MapDataRecordFields(IDataRecord dataRecord, IKey key,
            Dictionary<string, string> primaryKeyValues, Dictionary<string, string> fieldData);

        IDataRecord MapDataRecordFields(IDataRecord dataRecord, Dictionary<string, string> fieldData);

        Dictionary<string, string> MapFieldDictionaryFromRecord(IDataRecord dataRecord);
    }
}
