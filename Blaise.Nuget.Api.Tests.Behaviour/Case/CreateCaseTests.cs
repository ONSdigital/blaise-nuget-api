namespace Blaise.Nuget.Api.Tests.Behaviour.Case
{
    using System.Collections.Generic;
    using Blaise.Nuget.Api.Api;
    using Blaise.Nuget.Api.Contracts.Enums;
    using Blaise.Nuget.Api.Contracts.Extensions;
    using NUnit.Framework;

    public class CreateCaseTests
    {
        private readonly BlaiseCaseApi _sut;

        public CreateCaseTests()
        {
            _sut = new BlaiseCaseApi();
        }

        [Ignore("Integration")]
        [Test]
        public void Given_Valid_Arguments_When_I_Call_CreateCase_Then_The_Case_Is_Created()
        {
            // arrange
            const string serverParkName = "gusty";
            const string questionnaireName = "OPN2202_TST";
            var primaryKeyValues = new Dictionary<string, string> { { "QID.Serial_Number", "900001" } };
            var fieldData = new Dictionary<string, string>
            {
                { FieldNameType.HOut.FullName(), "110" },
                { FieldNameType.TelNo.FullName(), "07000000000" }
            };

            // act
            _sut.CreateCase(primaryKeyValues, fieldData, questionnaireName, serverParkName);

            // assert
            Assert.That(_sut.CaseExists(primaryKeyValues, questionnaireName, serverParkName), Is.True);

            // cleanup
            _sut.RemoveCase(primaryKeyValues, questionnaireName, serverParkName);
        }

        [Ignore("Integration")]
        [Test]
        public void Given_Valid_MultiKey_Arguments_When_I_Call_CreateCase_Then_The_Case_Is_Created()
        {
            // arrange
            const string serverParkName = "cma";
            const string questionnaireName = "CMA_Launcher";
            var primaryKeyValues = new Dictionary<string, string> { { "MainSurveyID", "7bded891-3aa6-41b2-824b-0be514018806" }, { "ID", "900001" } };

            var fieldData = new Dictionary<string, string>
            {
                { FieldNameType.HOut.FullName(), "110" },
                { FieldNameType.TelNo.FullName(), "07000000000" }
            };

            // act
            _sut.CreateCase(primaryKeyValues, fieldData, questionnaireName, serverParkName);

            // assert
            Assert.That(_sut.CaseExists(primaryKeyValues, questionnaireName, serverParkName), Is.True);

            // cleanup
            _sut.RemoveCase(primaryKeyValues, questionnaireName, serverParkName);
        }
    }
}
