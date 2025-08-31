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
        public void Given_I_Call_UpdateQuestionnaireFileWithAuditData_Then_The_Questionnaire_Is_Updated()
        {
            // arrange
            const string serverParkName = "LocalDevelopment";
            const string questionnaireName = "lms2301_ts6";
            const string questionnaireFile = @"C:\Temp\LMS2301_TS61.bpkg";

            CreateCases(100, questionnaireName, serverParkName);

            // act and assert
            Assert.DoesNotThrow(() => _sut.UpdateQuestionnaireFileWithData(serverParkName, questionnaireName, questionnaireFile, true));

            // cleanup
            DeleteCasesInDatabase(questionnaireName, serverParkName);
        }

        [Ignore("Integration")]
        [Test]
        public void Given_I_Call_UpdateQuestionnaireFileWithData_Then_The_Questionnaire_Is_Updated()
        {
            // arrange
            const string serverParkName = "LocalDevelopment";
            const string questionnaireName = "lms2301_ts6";
            const string questionnaireFile = @"C:\Temp\LMS2301_TS61.bpkg";

            CreateCases(100, questionnaireName, serverParkName);

            // act and assert
            Assert.DoesNotThrow(() => _sut.UpdateQuestionnaireFileWithData(serverParkName, questionnaireName, questionnaireFile));

            // cleanup
            DeleteCasesInDatabase(questionnaireName, serverParkName);
        }

        [Ignore("Integration")]
        [Test]
        public void Given_I_Call_UpdateQuestionnaireFileWithSqlConnection_Then_The_Questionnaire_Is_Updated()
        {
            // arrange
            const string questionnaireName = "LMS2101_AA1";
            const string questionnaireFile = @"D:\Blaise\Questionnaires\LMS2101_AA1.bpkg";

            // act and assert
            Assert.DoesNotThrow(() => _sut.UpdateQuestionnaireFileWithSqlConnection(
                questionnaireName,
                questionnaireFile));
        }

        [Ignore("Integration")]
        [Test]
        public void Given_I_Call_CreateSettingsDataInterfaceFile_Then_The_Interface_Is_Created()
        {
            // arrange
            const ApplicationType applicationType = ApplicationType.Cati;
            const string fileName = @"D:\OPN2101A.bcdi";

            // act and assert
            Assert.DoesNotThrow(() => _sut.CreateSettingsDataInterfaceFile(applicationType, fileName));
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
