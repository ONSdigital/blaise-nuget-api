namespace Blaise.Nuget.Api.Tests.Behaviour.File
{
    using Blaise.Nuget.Api.Api;
    using NUnit.Framework;

    public class FileServiceTests
    {
        private readonly BlaiseFileApi _sut;

        public FileServiceTests()
        {
            _sut = new BlaiseFileApi();
        }

        [Ignore("Integration")]
        [Test]
        public void When_I_Call_UpdateQuestionnaireFileWithData_Then_The_File_Is_Populated()
        {
            // arrange
            const string ServerParkName = "gusty";
            const string QuestionnaireName = "LMS2405_HU1";
            const string QuestionnaireFile = @"D:\Filter\LMS2405_HU1.zip";

            _sut.UpdateQuestionnaireFileWithSqlConnection(QuestionnaireName, QuestionnaireFile);

            // act and assert
            _sut.UpdateQuestionnaireFileWithData(ServerParkName, QuestionnaireName, QuestionnaireFile, false);
        }

        [Ignore("Integration")]
        [Test]
        public void Given_The_OverwriteExistingData_Argument_Is_True_When_I_Call_UpdateQuestionnaireFileWithSqlConnection_Then_The_Data_Is_Overwritten()
        {
            // arrange
            const string QuestionnaireName = "FRS2504A";
            const string QuestionnaireFile = @"D:\FRS2504A.bpkg";

            // act and assert
            _sut.UpdateQuestionnaireFileWithSqlConnection(QuestionnaireName, QuestionnaireFile, true);
        }
    }
}
