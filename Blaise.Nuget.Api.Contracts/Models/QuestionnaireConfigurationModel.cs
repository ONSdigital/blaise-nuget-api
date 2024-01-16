using Blaise.Nuget.Api.Contracts.Enums;

namespace Blaise.Nuget.Api.Contracts.Models
{
    public class QuestionnaireConfigurationModel
    {
        public QuestionnaireDataEntryType QuestionnaireDataEntryType { get; set; }

        public QuestionnaireInterviewType QuestionnaireInterviewType { get; set; }
    }
}
