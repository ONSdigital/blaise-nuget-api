
using Blaise.Nuget.Api.Api;
using Blaise.Nuget.Api.Contracts.Enums;
using Blaise.Nuget.Api.Contracts.Extensions;
using NUnit.Framework;
using System.Collections.Generic;
// ReSharper disable MissingXmlDoc

namespace Blaise.Nuget.Api.Tests.Behaviour.Case
{
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
            // Arrange
            const string serverParkName = "gusty";
            const string questionnaireName = "DST2304Z";
            var primaryKeyValues = new Dictionary<string, string> { { "QID.Serial_Number", "900001" } };
            var fieldData = new Dictionary<string, string>
            {
                {FieldNameType.HOut.FullName(), "110"},
                {FieldNameType.TelNo.FullName(), "07000000000"}
            };

            _sut.CreateCase(primaryKeyValues, fieldData, questionnaireName, serverParkName);

            // Act
            var dataRecord = _sut.GetCase(primaryKeyValues, questionnaireName, serverParkName);
            var result = _sut.GetCaseStatus(dataRecord);

            // Assert
            Assert.That("900001", Is.EqualTo(result.PrimaryKey));

            // Cleanup
            _sut.RemoveCase(primaryKeyValues, questionnaireName, serverParkName);
        }

        [Ignore("Integration")]
        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetCaseStatus_Then_The_CaseStatuses_Are_Returned_For_A_MultiKey_Questionnaire()
        {
            // Arrange
            const string serverParkName = "cma";
            const string questionnaireName = "CMA_Launcher";
            var primaryKeyValues = new Dictionary<string, string> { { "MainSurveyID", "7bded891-3aa6-41b2-824b-0be514018806" }, { "ID", "900001" } };
            var fieldData = new Dictionary<string, string>
            {
                {FieldNameType.HOut.FullName(), "110"},
                {FieldNameType.TelNo.FullName(), "07000000000"}
            };

            _sut.CreateCase(primaryKeyValues, fieldData, questionnaireName, serverParkName);

            // Act
            var dataRecord = _sut.GetCase(primaryKeyValues, questionnaireName, serverParkName);
            var result = _sut.GetCaseStatus(dataRecord);

            // Assert=
            Assert.That("900001", Is.EqualTo(result.PrimaryKey));

            // Cleanup
            _sut.RemoveCase(primaryKeyValues, questionnaireName, serverParkName);
        }

    }
}
