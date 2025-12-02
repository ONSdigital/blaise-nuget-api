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
        public void When_I_Call_GetCaseStatus_Then_The_CaseStatus_Is_Returned()
        {
            // arrange
            const string ServerParkName = "gusty";
            const string QuestionnaireName = "DST2304Z";
            var primaryKeyValues = new Dictionary<string, string> { { "QID.Serial_Number", "900001" } };
            var fieldData = new Dictionary<string, string>
            {
                { FieldNameType.HOut.FullName(), "110" },
                { FieldNameType.TelNo.FullName(), "07000000000" },
            };

            _sut.CreateCase(primaryKeyValues, fieldData, QuestionnaireName, ServerParkName);

            // act
            var dataRecord = _sut.GetCase(primaryKeyValues, QuestionnaireName, ServerParkName);
            var result = _sut.GetCaseStatus(dataRecord);

            // assert
            Assert.That("900001", Is.EqualTo(result.PrimaryKey));

            // cleanup
            _sut.RemoveCase(primaryKeyValues, QuestionnaireName, ServerParkName);
        }

        [Ignore("Integration")]
        [Test]
        public void When_I_Call_GetCaseStatus_Then_The_CaseStatus_Is_Returned_For_A_MultiKey_Questionnaire()
        {
            // arrange
            const string ServerParkName = "cma";
            const string QuestionnaireName = "CMA_Launcher";
            var primaryKeyValues = new Dictionary<string, string> { { "MainSurveyID", "7bded891-3aa6-41b2-824b-0be514018806" }, { "ID", "900001" } };
            var fieldData = new Dictionary<string, string>
            {
                { FieldNameType.HOut.FullName(), "110" },
                { FieldNameType.TelNo.FullName(), "07000000000" },
            };

            _sut.CreateCase(primaryKeyValues, fieldData, QuestionnaireName, ServerParkName);

            // act
            var dataRecord = _sut.GetCase(primaryKeyValues, QuestionnaireName, ServerParkName);
            var result = _sut.GetCaseStatus(dataRecord);

            // assert
            Assert.That(result.PrimaryKeyValues.Count, Is.EqualTo(2));
            Assert.That(result.PrimaryKeyValues["MainSurveyID"], Is.EqualTo("7bded891-3aa6-41b2-824b-0be514018806"));
            Assert.That(result.PrimaryKeyValues["ID"], Is.EqualTo("900001"));

            // cleanup
            _sut.RemoveCase(primaryKeyValues, QuestionnaireName, ServerParkName);
        }
    }
}
