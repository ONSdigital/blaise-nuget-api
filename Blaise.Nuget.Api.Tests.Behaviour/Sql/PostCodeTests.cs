namespace Blaise.Nuget.Api.Tests.Behaviour.Sql
{
    using Blaise.Nuget.Api.Api;
    using NUnit.Framework;

    public class PostCodeTests
    {
        private readonly BlaiseSqlApi _sut;

        public PostCodeTests()
        {
            _sut = new BlaiseSqlApi();
        }

        [Ignore("Integration")]
        [Test]
        public void Given_A_Questionnaire_Name_And_Primary_Key_When_I_Call_GetPostCode_Then_The_Expected_PostCode_Is_Returned()
        {
            // arrange
            const string QuestionnaireName = "dst2106A";
            const string PrimaryKey = "1005101";

            // act
            var result = _sut.GetPostCode(QuestionnaireName, PrimaryKey);

            // assert
            Assert.That(result, Is.EqualTo("NP899XX"));
        }
    }
}
