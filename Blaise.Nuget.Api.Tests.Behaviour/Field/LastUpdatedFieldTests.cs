namespace Blaise.Nuget.Api.Tests.Behaviour.Field
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Blaise.Nuget.Api.Api;
    using Blaise.Nuget.Api.Contracts.Enums;
    using Blaise.Nuget.Api.Contracts.Extensions;
    using NUnit.Framework;

    public class LastUpdatedFieldTests
    {
        private readonly BlaiseCaseApi _sut;

        private readonly Dictionary<string, string> _primaryKeyValues;

        public LastUpdatedFieldTests()
        {
            _sut = new BlaiseCaseApi();
            _primaryKeyValues = new Dictionary<string, string> { { "QID.Serial_Number", "9001" } };
        }

        [Ignore("Integration")]
        [Test]
        public void Given_Value_Set_When_I_Call_GetLastUpdatedDateTime_Then_The_Correct_Value_Is_Returned()
        {
            // arrange
            const string serverParkName = "LocalDevelopment";
            const string questionnaireName = "OPN2102R";
            const string dateValue = "02-12-2021";
            const string timeValue = "09:23:59";

            var lastUpdated = DateTime.ParseExact($"{dateValue} {timeValue}", "dd-MM-yyyy hh:mm:ss", CultureInfo.InvariantCulture);

            var fieldData = new Dictionary<string, string>
            {
                { FieldNameType.HOut.FullName(), "110" },
                { FieldNameType.TelNo.FullName(), "07000000000" },
                { FieldNameType.LastUpdatedDate.FullName(), dateValue },
                { FieldNameType.LastUpdatedTime.FullName(), timeValue }
            };

            _sut.CreateCase(_primaryKeyValues, fieldData, questionnaireName, serverParkName);

            // act
            var dataRecord = _sut.GetCase(_primaryKeyValues, questionnaireName, serverParkName);

            var result = _sut.GetLastUpdated(dataRecord);

            // arrange
            Assert.That(result, Is.EqualTo(lastUpdated));

            // cleanup
            _sut.RemoveCase(_primaryKeyValues, questionnaireName, serverParkName);
        }

        [Ignore("Integration")]
        [Test]
        public void Given_A_Questionnaire_Has_A_Field_When_I_Call_FieldExists_Then_The_Expected_Result_Is_Returned()
        {
            // arrange
            const string serverParkName = "gusty";
            const string questionnaireName = "LMS2209_EM1";
            const string fieldName = "QHAdmin.HOut";
            var dataRecord = _sut.GetCase(_primaryKeyValues, questionnaireName, serverParkName);

            // act
            var result = _sut.FieldExists(dataRecord, fieldName);

            // arrange
            Assert.That(result, Is.Not.Null);
        }
    }
}
