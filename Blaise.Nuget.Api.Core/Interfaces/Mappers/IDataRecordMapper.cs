namespace Blaise.Nuget.Api.Core.Interfaces.Mappers
{
    using StatNeth.Blaise.API.DataRecord;
    using System.Collections.Generic;

    public interface IDataRecordMapper
    {
        IDataRecord MapDataRecordFields(
            IDataRecord dataRecord,
            IKey key,
            Dictionary<string, string> primaryKeyValues,
            Dictionary<string, string> fieldData);

        IDataRecord MapDataRecordFields(IDataRecord dataRecord, Dictionary<string, string> fieldData);

        Dictionary<string, string> MapFieldDictionaryFromRecord(IDataRecord dataRecord);
    }
}
