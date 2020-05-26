﻿using StatNeth.Blaise.API.DataLink;
using StatNeth.Blaise.API.DataRecord;
using StatNeth.Blaise.API.Meta;
using System;
using System.Collections.Generic;
using Blaise.Nuget.Api.Contracts.Enums;
using StatNeth.Blaise.API.ServerManager;

namespace Blaise.Nuget.Api.Contracts.Interfaces
{
    public interface IFluentBlaiseRemoteApi
    {
        IFluentBlaiseRemoteApi WithServerPark(string serverParkName);

        IEnumerable<string> GetSurveyNames();

        IEnumerable<ISurvey> GetSurveys();

        IFluentBlaiseRemoteApi ForInstrument(string instrumentName);

        bool KeyExists(IKey key);

        Guid GetInstrumentId();

        IDatamodel GetDataModel();

        CaseRecordType GetCaseRecordType();

        IDataSet GetDataSet();

        IDataRecord GetDataRecord(IKey key);

        void WriteDataRecord(IDataRecord dataRecord);

        bool CompletedFieldExists();

        void MarkCaseAsComplete(IDataRecord dataRecord);

        bool ProcessedFieldExists();

        void MarkCaseAsProcessed(IDataRecord dataRecord);
    }
}
