namespace Blaise.Nuget.Api.Contracts.Models
{
    using Blaise.Nuget.Api.Contracts.Enums;

    public class QuestionnaireConfigurationModel
    {
        public QuestionnaireDataEntryType QuestionnaireDataEntryType { get; set; }

        public QuestionnaireInterviewType QuestionnaireInterviewType { get; set; }
    }
}
