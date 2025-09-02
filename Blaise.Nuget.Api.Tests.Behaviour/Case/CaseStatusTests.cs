namespace Blaise.Nuget.Api.Tests.Behaviour.Case
{
    using System.Collections.Generic;
    using Blaise.Nuget.Api.Api;
    using Blaise.Nuget.Api.Contracts.Enums;
    using Blaise.Nuget.Api.Contracts.Extensions;
    using NUnit.Framework;

    public class CaseStatusTests
    {
        private readonly BlaiseCaseApi _sut;

        public CaseStatusTests()
        {
            _sut = new BlaiseCaseApi();
        }

        [Ignore("Integration")]
        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetCaseStatus_Then_The_CaseStatuses_Are_Returned()
        {
            // arrange
            const string serverParkName = "gusty";
            const string questionnaireName = "DST2304Z";
            var primaryKeyValues = new Dictionary<string, string> { { "QID.Serial_Number", "900001" } };
            var fieldData = new Dictionary<string, string>
            {
                { FieldNameType.HOut.FullName(), "110" },
                { FieldNameType.TelNo.FullName(), "07000000000" },
            };

            _sut.CreateCase(primaryKeyValues, fieldData, questionnaireName, serverParkName);

            // act
            var dataRecord = _sut.GetCase(primaryKeyValues, questionnaireName, serverParkName);
            var result = _sut.GetCaseStatus(dataRecord);

            // assert
            Assert.That("900001", Is.EqualTo(result.PrimaryKey));

            // Cleanup
            _sut.RemoveCase(primaryKeyValues, questionnaireName, serverParkName);
        }

        [Ignore("Integration")]
        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetCaseStatus_Then_The_CaseStatuses_Are_Returned_For_A_MultiKey_Questionnaire()
        {
            // arrange
            const string serverParkName = "cma";
            const string questionnaireName = "CMA_Launcher";
            var primaryKeyValues = new Dictionary<string, string> { { "MainSurveyID", "7bded891-3aa6-41b2-824b-0be514018806" }, { "ID", "900001" } };
            var fieldData = new Dictionary<string, string>
            {
                { FieldNameType.HOut.FullName(), "110" },
                { FieldNameType.TelNo.FullName(), "07000000000" },
            };

            _sut.CreateCase(primaryKeyValues, fieldData, questionnaireName, serverParkName);

            // act
            var dataRecord = _sut.GetCase(primaryKeyValues, questionnaireName, serverParkName);
            var result = _sut.GetCaseStatus(dataRecord);

            // assert=
            Assert.That(result.PrimaryKeyValues.Count, Is.EqualTo(2));
            Assert.That(result.PrimaryKeyValues["MainSurveyID"], Is.EqualTo("7bded891-3aa6-41b2-824b-0be514018806"));
            Assert.That(result.PrimaryKeyValues["ID"], Is.EqualTo("900001"));

            // Cleanup
            _sut.RemoveCase(primaryKeyValues, questionnaireName, serverParkName);
        }
    }
}
