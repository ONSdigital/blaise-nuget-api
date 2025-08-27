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
        public void Given_A_QuestionnaireName_When_I_Call_GetCaseIds_I_Get_A_List_Of_CaseIds_Back()
        {
            //arrange
            const string questionnaireName = "OPN2105F";

            //act
            var result = _sut.GetCaseIds(questionnaireName);

            //assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
        }

        [Ignore("Integration")]
        [Test]
        public void Given_A_QuestionnaireName_When_I_Call_GetCaseIdentifiers_I_Get_A_List_Of_CaseIdentifiers_Back()
        {
            //arrange
            const string questionnaireName = "OPN2105F";

            //act
            var result = _sut.GetCaseIdentifiers(questionnaireName);

            //assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
        }
    }
}
