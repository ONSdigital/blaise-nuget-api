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
        public void DropQuestionnaireTables_WithValidQuestionnaireName_ShouldDropTables()
        {
            // arrange
            const string questionnaireName = "LMS2211_EJ2";

            // act
            var result = _sut.DropQuestionnaireTables(questionnaireName);

            // assert
            Assert.That(result, Is.True, "Expected tables were dropped given a valid questionnaire name.");
        }

        [Ignore("Integration")]
        [Test]
        public void DropQuestionnaireTables_ShouldExecuteSuccessfully_WithNonexistentQuestionnaireName()
        {
            // arrange
            const string questionnaireName = "xxxxxxx";

            // act
            var result = _sut.DropQuestionnaireTables(questionnaireName);

            // assert
            Assert.That(result, Is.True, "Expected successful execution for a nonexistent questionnaire name.");
        }
    }
}
