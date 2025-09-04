namespace Blaise.Nuget.Api.Contracts.Enums
{
    using System.ComponentModel;

    public enum SqlFieldType
    {
        [Description("Serial_Number")]
        CaseId,

        [Description("QDataBag_PostCode")]
        PostCode,

        [Description("QEdit_LastUpdated")]
        EditLastUpdated,

        [Description("QEdit_edited")]
        Edited,
    }
}
