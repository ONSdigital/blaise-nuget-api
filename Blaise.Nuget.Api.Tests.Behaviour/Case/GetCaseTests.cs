namespace Blaise.Nuget.Api.Tests.Behaviour.Case
{
    using System.Collections.Generic;
    using Blaise.Nuget.Api.Api;
    using Blaise.Nuget.Api.Contracts.Enums;
    using Blaise.Nuget.Api.Contracts.Extensions;
    using NUnit.Framework;

    public class GetCaseTests
    {
        private readonly BlaiseCaseApi _sut;

        private readonly Dictionary<string, string> _primaryKeyValues;

        public GetCaseTests()
        {
            _sut = new BlaiseCaseApi();
            _primaryKeyValues = new Dictionary<string, string> { { "QID.Serial_Number", "900001" } };
        }

        [Ignore("Integration")]
        [Test]
        public void When_I_Call_GetCase_Then_The_Case_Is_Returned()
        {
            // arrange
            const string ServerParkName = "LocalDevelopment";
            const string QuestionnaireName = "OPN2102R";
            var fieldData = new Dictionary<string, string>
            {
                { FieldNameType.HOut.FullName(), "110" },
                { FieldNameType.TelNo.FullName(), "07000000000" },
            };

            _sut.CreateCase(_primaryKeyValues, fieldData, QuestionnaireName, ServerParkName);

            // act
            var result = _sut.GetCase(_primaryKeyValues, QuestionnaireName, ServerParkName);

            // assert
            Assert.That(_sut.GetPrimaryKeyValues(result), Is.EqualTo(_primaryKeyValues));

            // cleanup
            _sut.RemoveCase(_primaryKeyValues, QuestionnaireName, ServerParkName);
        }

        [Ignore("Integration")]
        [Test]
        public void Given_Cases_Exist_When_I_Specify_A_Filter_Then_The_Expected_Cases_Are_Returned()
        {
            // arrange
            const string ServerParkName = "gusty";
            const string QuestionnaireName = "LMS2405_HU1";
            const string Filter = "Id=10";

            // act
            var result = _sut.GetFilteredCases(QuestionnaireName, ServerParkName, Filter);

            // assert
            Assert.That(result.RecordCount, Is.EqualTo(5));
        }
    }
}
