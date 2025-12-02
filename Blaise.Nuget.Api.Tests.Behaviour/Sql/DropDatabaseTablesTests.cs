namespace Blaise.Nuget.Api.Tests.Behaviour.Sql
{
    using Blaise.Nuget.Api.Api;
    using NUnit.Framework;

    public class DropDatabaseTablesTests
    {
        private readonly BlaiseSqlApi _sut;

        public DropDatabaseTablesTests()
        {
            _sut = new BlaiseSqlApi();
        }

        [Ignore("Integration")]
        [Test]
        public void Given_A_Valid_Questionnaire_Name_When_I_Call_DropQuestionnaireTables_Then_The_Tables_Are_Dropped()
        {
            // arrange
            const string QuestionnaireName = "LMS2211_EJ2";

            // act
            var result = _sut.DropQuestionnaireTables(QuestionnaireName);

            // assert
            Assert.That(result, Is.True, "Expected tables were dropped given a valid questionnaire name.");
        }

        [Ignore("Integration")]
        [Test]
        public void Given_A_Nonexistent_Questionnaire_Name_When_I_Call_DropQuestionnaireTables_Then_True_Is_Returned()
        {
            // arrange
            const string QuestionnaireName = "xxxxxxx";

            // act
            var result = _sut.DropQuestionnaireTables(QuestionnaireName);

            // assert
            Assert.That(result, Is.True, "Expected successful execution for a nonexistent questionnaire name.");
        }
    }
}
