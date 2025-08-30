namespace Blaise.Nuget.Api.Contracts.Enums
{
    using System.ComponentModel;

    public enum QuestionnaireInterviewType
    {
        [Description("CATI")]
        Cati,

        [Description("CAWI")]
        Cawi,

        [Description("CAPI")]
        Capi
    }
}
