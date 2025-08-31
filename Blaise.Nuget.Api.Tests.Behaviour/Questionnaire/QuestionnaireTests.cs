namespace Blaise.Nuget.Api.Tests.Behaviour.Questionnaire
{
    using System.Linq;
    using Blaise.Nuget.Api.Api;
    using Blaise.Nuget.Api.Contracts.Enums;
    using Blaise.Nuget.Api.Contracts.Extensions;
    using Blaise.Nuget.Api.Contracts.Models;
    using NUnit.Framework;
    using StatNeth.Blaise.API.ServerManager;

    public class QuestionnaireTests
    {
        private const string ServerParkName = "gusty";
        private const string FullQuestionnairePath = @"D:\Blaise\Instruments\DST2304Z.bpkg";
        private const string QuestionnaireName = "DST2304Z";

        private readonly BlaiseQuestionnaireApi _sut;

        public QuestionnaireTests()
        {
            _sut = new BlaiseQuestionnaireApi();
        }

        [Ignore("Integration")]
        [Test]
        public void Given_I_Have_A_Valid_Questionnaire_It_Gets_Installed_On_A_Server_Park()
        {
            // arrange
            var installOptions = new InstallOptions
            {
                DataEntrySettingsName = QuestionnaireDataEntryType.StrictInterviewing.ToString(),
                InitialAppLayoutSetGroupName = QuestionnaireInterviewType.Cati.FullName(),
                LayoutSetGroupName = QuestionnaireInterviewType.Cati.FullName(),
                OverwriteMode = DataOverwriteMode.Always
            };

            // act
            _sut.InstallQuestionnaire(QuestionnaireName, ServerParkName, FullQuestionnairePath, installOptions);

            // assert
            var questionnaires = _sut.GetQuestionnaires(ServerParkName).ToList();
            Assert.That(questionnaires.First(s => s.Name == QuestionnaireName), Is.Not.Null);
        }

        [Ignore("Integration")]
        [Test]
        public void Given_An_Questionnaire_Is_Installed_When_I_Call_GetQuestionnaire_The_Correct_Questionnaire_Is_Returned()
        {
            // act
            var result = _sut.GetQuestionnaire(QuestionnaireName, ServerParkName);

            // assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<ISurvey>());
            Assert.That(result.Name, Is.EqualTo(QuestionnaireName));
        }

        [Ignore("Integration")]
        [Test]
        public void Given_A_Questionnaire_Is_Installed_When_I_Call_Deactivate_Then_The_Questionnaire_Is_Deactivated()
        {
            // act
            _sut.DeactivateQuestionnaire(QuestionnaireName, ServerParkName);

            // assert
            Assert.That(_sut.GetQuestionnaireStatus(QuestionnaireName, ServerParkName), Is.EqualTo(QuestionnaireStatusType.Inactive));
        }

        [Ignore("Integration")]
        [Test]
        public void Given_A_Questionnaire_Is_Installed_When_I_Call_Activate_Then_The_Questionnaire_Is_Activated()
        {
            // act
            _sut.ActivateQuestionnaire(QuestionnaireName, ServerParkName);

            // assert
            Assert.That(_sut.GetQuestionnaireStatus(QuestionnaireName, ServerParkName), Is.EqualTo(QuestionnaireStatusType.Active));
        }

        [Ignore("Integration")]
        [Test]
        public void Given_A_Questionnaire_Is_Installed_In_Cati_And_Cawi_Mode_When_I_Call_GetQuestionnaireModes_The_Correct_Modes_Are_Returned()
        {
            // act
            var result = _sut.GetQuestionnaireModes(QuestionnaireName, ServerParkName).ToList();

            // assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result.Contains("CAWI"), Is.True);
            Assert.That(result.Contains("CATI"), Is.True);
        }

        [Ignore("Integration")]
        [Test]
        public void Given_A_Questionnaire_Is_Installed_In_When_I_Call_GetQuestionnaireDataEntrySettings_I_Get_A_Settings_Model_Back()
        {
            // act
            var result = _sut.GetQuestionnaireDataEntrySettings(QuestionnaireName, ServerParkName);

            // assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<DataEntrySettingsModel>());
        }
    }
}
