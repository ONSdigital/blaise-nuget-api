using System.ComponentModel;

namespace Blaise.Nuget.Api.Contracts.Enums
{
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
