namespace Blaise.Nuget.Api.Tests.Behaviour.Field
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Blaise.Nuget.Api.Api;
    using Blaise.Nuget.Api.Contracts.Enums;
    using Blaise.Nuget.Api.Contracts.Extensions;
    using NUnit.Framework;

    public class FieldTests
    {
        private readonly BlaiseCaseApi _sut;

        private readonly Dictionary<string, string> _primaryKeyValues;

        public FieldTests()
        {
            _sut = new BlaiseCaseApi();
            _primaryKeyValues = new Dictionary<string, string> { { "QID.Serial_Number", "9000001" } };
        }

        [Ignore("Integration")]
        [Test]
        public void Given_LastUpdated_Values_Are_Set_When_I_Call_GetLastUpdated_Then_The_Correct_Value_Is_Returned()
        {
            // arrange
            const string ServerParkName = "LocalDevelopment";
            const string QuestionnaireName = "OPN2102R";
            const string DateValue = "02-12-2021";
            const string TimeValue = "09:23:59";

            var lastUpdated = DateTime.ParseExact($"{DateValue} {TimeValue}", "dd-MM-yyyy hh:mm:ss", CultureInfo.InvariantCulture);

            var fieldData = new Dictionary<string, string>
            {
                { FieldNameType.HOut.FullName(), "110" },
                { FieldNameType.TelNo.FullName(), "07000000000" },
                { FieldNameType.LastUpdatedDate.FullName(), DateValue },
                { FieldNameType.LastUpdatedTime.FullName(), TimeValue },
            };

            _sut.CreateCase(_primaryKeyValues, fieldData, QuestionnaireName, ServerParkName);

            // act
            var dataRecord = _sut.GetCase(_primaryKeyValues, QuestionnaireName, ServerParkName);

            var result = _sut.GetLastUpdated(dataRecord);

            // assert
            Assert.That(result, Is.EqualTo(lastUpdated));

            // cleanup
            _sut.RemoveCase(_primaryKeyValues, QuestionnaireName, ServerParkName);
        }

        [Ignore("Integration")]
        [Test]
        public void Given_A_Field_Value_Is_Set_When_I_Call_GetFieldValue_Then_The_Correct_Value_Is_Returned()
        {
            // arrange
            const string ServerParkName = "LocalDevelopment";
            const string QuestionnaireName = "OPN2102R";
            const string TelNoValue = "07000000000";

            var fieldData = new Dictionary<string, string>
            {
                { FieldNameType.HOut.FullName(), "110" },
                { FieldNameType.TelNo.FullName(), TelNoValue },
            };

            _sut.CreateCase(_primaryKeyValues, fieldData, QuestionnaireName, ServerParkName);

            // act
            var dataRecord = _sut.GetCase(_primaryKeyValues, QuestionnaireName, ServerParkName);

            var result = _sut.GetFieldValue(dataRecord, FieldNameType.TelNo);

            // assert
            Assert.That(result.ValueAsText, Is.EqualTo(TelNoValue));

            // cleanup
            _sut.RemoveCase(_primaryKeyValues, QuestionnaireName, ServerParkName);
        }
    }
}
