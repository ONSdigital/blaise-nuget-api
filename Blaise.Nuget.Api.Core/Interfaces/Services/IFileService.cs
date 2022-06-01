using Blaise.Nuget.Api.Contracts.Models;
using StatNeth.Blaise.API.DataInterface;

namespace Blaise.Nuget.Api.Core.Interfaces.Services
{
    public interface IFileService
    {
        void UpdateQuestionnaireFileWithData(ConnectionModel connectionModel, string questionnaireFile,
            string questionnaireName, string serverParkName);

        void UpdateQuestionnairePackageWithSqlConnection(string questionnaireName,
            string questionnaireFile);

        void CreateSettingsDataInterfaceFile(ApplicationType applicationType, string fileName);
    }
}