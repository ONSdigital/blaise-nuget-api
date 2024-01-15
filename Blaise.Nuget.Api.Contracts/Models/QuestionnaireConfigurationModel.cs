using Blaise.Nuget.Api.Contracts.Enums;

namespace Blaise.Nuget.Api.Contracts.Models
{
    public class QuestionnaireConfigurationModel
    {
        public QuestionnaireDataEntryType InitialDataEntrySettingsName { get; set; }

        public QuestionnaireInterviewType InitialLayoutSetGroupName { get; set; }
    }
}
