﻿using System.ComponentModel;

namespace Blaise.Nuget.Api.Contracts.Enums
{
    public enum SqlFieldType
    {
        [Description("Serial_Number")]
        CaseId,

        [Description("QDataBag_PostCode")]
        PostCode
    }
}