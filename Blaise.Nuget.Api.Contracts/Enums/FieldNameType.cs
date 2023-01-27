using System.ComponentModel;

namespace Blaise.Nuget.Api.Contracts.Enums
{
    public enum FieldNameType
    {
        [Description("QHAdmin.HOut")]
        HOut,

        [Description("Mode")]
        Mode,

        [Description("QDataBag.TelNo")]
        TelNo,

        [Description("QDataBag.TelNo2")]
        TelNo2,

        [Description("DateTimeStamp")]
        LastUpdated,

        [Description("DateStamp")]
        LastUpdatedDate,

        [Description("TimeStamp")]
        LastUpdatedTime,

        [Description("QDataBag.PostCode")]
        PostCode
    }
}