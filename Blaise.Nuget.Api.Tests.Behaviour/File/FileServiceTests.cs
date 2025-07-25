using Blaise.Nuget.Api.Api;
using NUnit.Framework;

namespace Blaise.Nuget.Api.Tests.Behaviour.File
{
    public class FileServiceTests
    {
        private readonly BlaiseFileApi _sut;

        public FileServiceTests()
        {
            _sut = new BlaiseFileApi();
        }

        [Ignore("Integration")]
        [Test]
        public void Given_Valid_Arguments_When_I_Call_UpdateQuestionnaireFileWithData_Then_The_File_is_Populated()
        {
            //arrange
            const string serverParkName = "gusty";
            const string questionnaireName = "LMS2405_HU1";
            const string questionnaireFile = @"D:\Filter\LMS2405_HU1.zip";

            _sut.UpdateQuestionnaireFileWithSqlConnection(questionnaireName, questionnaireFile);

            //act && assert
            _sut.UpdateQuestionnaireFileWithData(serverParkName, questionnaireName, questionnaireFile, false);
        }

        [Ignore("Integration")]
        [Test]
        public void Given_The_OverwriteExistingData_Argument_Is_True_When_I_Call_UpdateQuestionnaireFileWithSqlConnection_Then_The_Data_Is_Overwritten()
        {
            //arrange            
            const string questionnaireName = "FRS2504A";
            const string questionnaireFile = @"D:\FRS2504A.bpkg";

            //act && assert
            _sut.UpdateQuestionnaireFileWithSqlConnection(questionnaireName, questionnaireFile, true);

        }
    }
}
