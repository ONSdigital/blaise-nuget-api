namespace Blaise.Nuget.Api.Contracts.Enums
{
    using System.ComponentModel;

    public enum FieldNameType
    {
        [Description("QHAdmin.HOut")]
        HOut,

        [Description("Mode")]
        Mode,

        [Description("QDataBag.TelNo")]
        TelNo,

        [Description("DateTimeStamp")]
        LastUpdated,

        [Description("DateStamp")]
        LastUpdatedDate,

        [Description("TimeStamp")]
        LastUpdatedTime,

        [Description("QDataBag.PostCode")]
        PostCode,
    }
}
