﻿using System.Collections.Generic;
using Blaise.Nuget.Api.Contracts.Enums;
using StatNeth.Blaise.API.DataLink;
using StatNeth.Blaise.API.DataRecord;

namespace Blaise.Nuget.Api.Contracts.Interfaces
{
    public interface IBlaiseCaseApi
    {
        bool CaseExists(string primaryKeyValue, string instrumentName, 
            string serverParkName);

        string GetPrimaryKeyValue(IDataRecord dataRecord);
        IDataSet GetCases(string databaseFile);
        IDataSet GetCases(string instrumentName, string serverParkName);

        IDataRecord GetCase(string primaryKeyValue, string instrumentName, 
            string serverParkName);

        void CreateCase(string primaryKeyValue, Dictionary<string, string> fieldData, 
            string instrumentName, string serverParkName);

        void CreateCase(string databaseFile, string primaryKeyValue, Dictionary<string, string> fieldData);

        void UpdateCase(IDataRecord dataRecord, Dictionary<string, string> fieldData,
            string instrumentName, string serverParkName);

        bool FieldExists(string instrumentName, string serverParkName, FieldNameType fieldNameType);
        bool FieldExists(IDataRecord dataRecord, FieldNameType fieldNameType);
        IDataValue GetFieldValue(IDataRecord dataRecord, FieldNameType fieldNameType);

        IDataValue GetFieldValue(string primaryKeyValue, string instrumentName,
            string serverParkName, FieldNameType fieldNameType);

        void RemoveCase(string primaryKeyValue, string instrumentName, 
            string serverParkName);

        int GetNumberOfCases(string instrumentName, string serverParkName);
        int GetNumberOfCases(string databaseFile);
    }
}