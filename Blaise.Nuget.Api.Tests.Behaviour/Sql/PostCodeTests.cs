using Blaise.Nuget.Api.Api;
using NUnit.Framework;

namespace Blaise.Nuget.Api.Tests.Behaviour.Sql
{
    public class PostCodeTests
    {
        private readonly BlaiseSqlApi _sut;
        
        public PostCodeTests()
        {
            _sut = new BlaiseSqlApi();
        }

        [Ignore("Integration")]
        [Test]
        public void Given_An_InstrumentName_And_PrimaryKey_When_I_Call_GetPostCode_I_Get_A_PostCode_Back()
        {
            //arrange
            const string instrumentName = "OPN2105F";
            const string primaryKey = "911467";

            //act
            var result = _sut.GetPostCode(instrumentName, primaryKey);

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual("AC58TST", result);
        }
    }
}
