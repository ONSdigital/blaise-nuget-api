using Blaise.Nuget.Api.Api;
using NUnit.Framework;

namespace Blaise.Nuget.Api.Tests.Behaviour.Sql
{
    public class CaseIdTests
    {
        private readonly BlaiseSqlApi _sut;
        
        public CaseIdTests()
        {
            _sut = new BlaiseSqlApi();
        }

        [Ignore("Integration")]
        [Test]
        public void Given_An_InstrumentName_When_I_Call_GetCaseIds_I_Get_A_List_Of_CaseIds_Back()
        {
            //arrange
            var instrumentName = "OPN2105F";

            //act
            var result = _sut.GetCaseIds(instrumentName);

            //assert
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
        }
    }
}
