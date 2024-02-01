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
        public void Given_An_QuestionnaireName_And_PrimaryKey_When_I_Call_GetPostCode_I_Get_A_PostCode_Back()
        {
            //arrange
            /* const string questionnaireName = "dst2106A";

             //act
             var result = _sut.

             //assert
             Assert.IsNotNull(result);
             Assert.AreEqual("NP899XX", result);*/
        }


    }
}
