namespace Blaise.Nuget.Api.Tests.Behaviour.QuestionnaireFile
{
    using System.Collections.Generic;
    using Blaise.Nuget.Api.Api;
    using NUnit.Framework;
    using StatNeth.Blaise.API.DataInterface;

    public class QuestionnaireFileTests
    {
        private readonly BlaiseFileApi _sut;

        public QuestionnaireFileTests()
        {
            _sut = new BlaiseFileApi();
        }

        [Ignore("Integration")]
        [Test]
        public void When_I_Call_UpdateQuestionnaireFileWithAuditData_Then_The_Questionnaire_Is_Updated()
        {
            // arrange
            const string ServerParkName = "LocalDevelopment";
            const string QuestionnaireName = "lms2301_ts6";
            const string QuestionnaireFile = @"C:\Temp\LMS2301_TS61.bpkg";

            CreateCases(100, QuestionnaireName, ServerParkName);

            // act and assert
            Assert.DoesNotThrow(() => _sut.UpdateQuestionnaireFileWithData(ServerParkName, QuestionnaireName, QuestionnaireFile, true));

            // cleanup
            DeleteCasesInDatabase(QuestionnaireName, ServerParkName);
        }

        [Ignore("Integration")]
        [Test]
        public void When_I_Call_UpdateQuestionnaireFileWithData_Then_The_Questionnaire_Is_Updated()
        {
            // arrange
            const string ServerParkName = "LocalDevelopment";
            const string QuestionnaireName = "lms2301_ts6";
            const string QuestionnaireFile = @"C:\Temp\LMS2301_TS61.bpkg";

            CreateCases(100, QuestionnaireName, ServerParkName);

            // act and assert
            Assert.DoesNotThrow(() => _sut.UpdateQuestionnaireFileWithData(ServerParkName, QuestionnaireName, QuestionnaireFile));

            // cleanup
            DeleteCasesInDatabase(QuestionnaireName, ServerParkName);
        }

        [Ignore("Integration")]
        [Test]
        public void When_I_Call_UpdateQuestionnaireFileWithSqlConnection_Then_The_Questionnaire_Is_Updated()
        {
            // arrange
            const string QuestionnaireName = "LMS2101_AA1";
            const string QuestionnaireFile = @"D:\Blaise\Questionnaires\LMS2101_AA1.bpkg";

            // act and assert
            Assert.DoesNotThrow(() => _sut.UpdateQuestionnaireFileWithSqlConnection(
                QuestionnaireName,
                QuestionnaireFile));
        }

        [Ignore("Integration")]
        [Test]
        public void When_I_Call_CreateSettingsDataInterfaceFile_Then_The_Interface_Is_Created()
        {
            // arrange
            const ApplicationType ApplicationType = ApplicationType.Cati;
            const string FileName = @"D:\OPN2101A.bcdi";

            // act and assert
            Assert.DoesNotThrow(() => _sut.CreateSettingsDataInterfaceFile(ApplicationType, FileName));
        }

        private static void CreateCases(int numberOfCases, string questionnaireName, string serverParkName)
        {
            var blaiseCaseApi = new BlaiseCaseApi();
            var primaryKey = 90000;

            for (var count = 0; count < numberOfCases; count++)
            {
                var dictionary = new Dictionary<string, string> { { "serial_number", primaryKey.ToString() } };

                var primaryKeyValues = new Dictionary<string, string> { { "QID.Serial_Number", primaryKey.ToString() } };
                blaiseCaseApi.CreateCase(primaryKeyValues, dictionary, questionnaireName, serverParkName);
                primaryKey++;
            }
        }

        private static void DeleteCasesInDatabase(string questionnaireName, string serverParkName)
        {
            var blaiseCaseApi = new BlaiseCaseApi();

            var cases = blaiseCaseApi.GetCases(questionnaireName, serverParkName);

            while (!cases.EndOfSet)
            {
                var primaryKey = blaiseCaseApi.GetPrimaryKeyValues(cases.ActiveRecord);

                blaiseCaseApi.RemoveCase(primaryKey, questionnaireName, serverParkName);

                cases.MoveNext();
            }
        }
    }
}
