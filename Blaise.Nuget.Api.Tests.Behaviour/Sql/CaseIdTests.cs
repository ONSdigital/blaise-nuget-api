namespace Blaise.Nuget.Api.Tests.Behaviour.Sql
{
    using Blaise.Nuget.Api.Api;
    using NUnit.Framework;

    public class CaseIdTests
    {
        private readonly BlaiseSqlApi _sut;

        public CaseIdTests()
        {
            _sut = new BlaiseSqlApi();
        }

        [Ignore("Integration")]
        [Test]
        public void Given_A_Questionnaire_Name_When_I_Call_GetCaseIds_Then_A_List_Of_Case_Ids_Is_Returned()
        {
            // arrange
            const string QuestionnaireName = "OPN2105F";

            // act
            var result = _sut.GetCaseIds(QuestionnaireName);

            // assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
        }

        [Ignore("Integration")]
        [Test]
        public void Given_A_Questionnaire_Name_When_I_Call_GetCaseIdentifiers_Then_A_List_Of_Case_Identifiers_Is_Returned()
        {
            // arrange
            const string QuestionnaireName = "OPN2105F";

            // act
            var result = _sut.GetCaseIdentifiers(QuestionnaireName);

            // assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
        }
    }
}
