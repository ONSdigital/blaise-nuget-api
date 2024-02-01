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
        public void Given_An_QuestionnaireName_Drop_The_Table_From_The_Database()
        {
            //arrange
            const string questionnaireName = "DST2304Z";

            //act
            var result = _sut.DropQuestionnaireTables(questionnaireName);

            //assert
            Assert.IsTrue(result);
        }


    }
}
