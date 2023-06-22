using StatNeth.Blaise.API.DataInterface;

namespace Blaise.Nuget.Api.Contracts.Interfaces
{
    public interface IBlaiseFileApi
    {
        void UpdateQuestionnaireFileWithData(string serverParkName, string questionnaireName, string questionnaireFile);

        void UpdateQuestionnaireFileWithSqlConnection(string questionnaireName, string questionnaireFile);

        void CreateSettingsDataInterfaceFile(ApplicationType applicationType, string fileName);

        void UpdateQuestionnaireFileWithData(string serverParkName, string questionnaireName
            , string questionnaireFile, bool auditOption);
    }
}