using Blaise.Nuget.Api.Api;
using NUnit.Framework;

namespace Blaise.Nuget.Api.Tests.Behaviour.Sql
{
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
            //Arrange
            const string questionnaireName = "LMS2211_EJ2";

            //Act
            var result = _sut.DropQuestionnaireTables(questionnaireName);

            //Assert
            Assert.IsTrue(result, "Expected tables were dropped given a valid questionnaire name.");

        }

        [Ignore("Integration")]
        [Test]
        public void DropQuestionnaireTables_ShouldExecuteSuccessfully_WithNonexistentQuestionnaireName()
        {
            //Arrange
            const string questionnaireName = "xxxxxxx";

            //Act
            var result = _sut.DropQuestionnaireTables(questionnaireName);

            //Assert
            Assert.IsTrue(result, "Expected successful execution for a nonexistent questionnaire name.");
        }
    }
}
