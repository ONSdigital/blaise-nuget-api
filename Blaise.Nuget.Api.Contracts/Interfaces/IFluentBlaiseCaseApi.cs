﻿using System.Collections.Generic;
using Blaise.Nuget.Api.Contracts.Enums;
using StatNeth.Blaise.API.DataRecord;

namespace Blaise.Nuget.Api.Contracts.Interfaces
{
    public interface IFluentBlaiseCaseApi
    {
        IFluentBlaiseCaseApi WithPrimaryKey(string primaryKeyValue);

        IFluentBlaiseCaseApi WithDataRecord(IDataRecord caseDataRecord);

        IFluentBlaiseCaseApi WithData(Dictionary<string, string> data);

        IFluentBlaiseCaseApi WithStatus(StatusType statusType);

        string PrimaryKey { get; }

        bool Completed { get; }

        bool Processed { get; }

        bool Exists { get; }

        void Add();

        void Update();

        void Remove();

        IFluentBlaiseHandler Copy { get; }

        IFluentBlaiseHandler Move { get; }
    }
}