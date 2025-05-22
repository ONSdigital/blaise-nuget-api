using System.Linq;
using Blaise.Nuget.Api.Api;
using Blaise.Nuget.Api.Contracts.Enums;
using Blaise.Nuget.Api.Contracts.Extensions;
using Blaise.Nuget.Api.Contracts.Models;
using NUnit.Framework;
using StatNeth.Blaise.API.ServerManager;

namespace Blaise.Nuget.Api.Tests.Behaviour.Questionnaire
{
    public class QuestionnaireTests
    {
        private readonly BlaiseQuestionnaireApi _sut;

        private const string ServerParkName = "gusty";
        private const string FullQuestionnairePath = @"D:\Blaise\Instruments\DST2304Z.bpkg";
        private const string QuestionnaireName = "DST2304Z";

        public QuestionnaireTests()
        {
            _sut = new BlaiseQuestionnaireApi();
        }

        [Ignore("Integration")]
        [Test]
        public void Given_I_Have_A_Valid_Questionnaire_It_Gets_Installed_On_A_Server_Park()
        {
            //arrange
            var installOptions = new InstallOptions
            {
                DataEntrySettingsName = QuestionnaireDataEntryType.StrictInterviewing.ToString(),
                InitialAppLayoutSetGroupName = QuestionnaireInterviewType.Cati.FullName(),
                LayoutSetGroupName = QuestionnaireInterviewType.Cati.FullName(),
                OverwriteMode = DataOverwriteMode.Always
            };

            //act
            _sut.InstallQuestionnaire(QuestionnaireName, ServerParkName, FullQuestionnairePath, installOptions);

            //assert
            var questionnaires = _sut.GetQuestionnaires(ServerParkName).ToList();
            Assert.IsNotNull(questionnaires.First(s => s.Name == QuestionnaireName));
        }

        [Ignore("Integration")]
        [Test]
        public void Given_An_Questionnaire_Is_Installed_When_I_Call_GetQuestionnaire_The_Correct_Questionnaire_Is_Returned()
        {
            //act
            var result = _sut.GetQuestionnaire(QuestionnaireName, ServerParkName);

            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ISurvey>(result);

            Assert.AreEqual(QuestionnaireName, result.Name);
        }

        [Ignore("Integration")]
        [Test]
        public void Given_A_Questionnaire_Is_Installed_When_I_Call_Deactivate_Then_The_Questionnaire_Is_Deactivated()
        {
            //act
            _sut.DeactivateQuestionnaire(QuestionnaireName, ServerParkName);

            //assert
            Assert.AreEqual(QuestionnaireStatusType.Inactive, _sut.GetQuestionnaireStatus(QuestionnaireName, ServerParkName));
        }

        [Ignore("Integration")]
        [Test]
        public void Given_A_Questionnaire_Is_Installed_When_I_Call_Activate_Then_The_Questionnaire_Is_Activated()
        {
            //act
            _sut.ActivateQuestionnaire(QuestionnaireName, ServerParkName);

            //assert
            Assert.AreEqual(QuestionnaireStatusType.Active, _sut.GetQuestionnaireStatus(QuestionnaireName, ServerParkName));
        }

        [Ignore("Integration")]
        [Test]
        public void Given_A_Questionnaire_Is_Installed_In_Cati_And_Cawi_Mode_When_I_Call_GetQuestionnaireModes_The_Correct_Modes_Are_Returned()
        {
            //act
            var result = _sut.GetQuestionnaireModes(QuestionnaireName, ServerParkName).ToList();

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.True(result.Contains("CAWI"));
            Assert.True(result.Contains("CATI"));
        }

        [Ignore("Integration")]
        [Test]
        public void Given_A_Questionnaire_Is_Installed_In_When_I_Call_GetQuestionnaireDataEntrySettings_I_Get_A_Settings_Model_Back()
        {
            //act
            var result = _sut.GetQuestionnaireDataEntrySettings(QuestionnaireName, ServerParkName);

            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<DataEntrySettingsModel>(result);
        }
    }
}
