using System;
using System.Collections.Generic;
using System.Globalization;
using Blaise.Nuget.Api.Api;
using Blaise.Nuget.Api.Contracts.Enums;
using Blaise.Nuget.Api.Contracts.Extensions;
using NUnit.Framework;

namespace Blaise.Nuget.Api.Tests.Behaviour.Field
{
    public class FieldTests
    {
        private readonly BlaiseCaseApi _sut;
        private readonly string _primaryKey;

        public FieldTests()
        {
            _sut = new BlaiseCaseApi();
            _primaryKey = "9000001";
        }

        [Ignore("Integration")]
        [Test]
        public void Given_Value_Set_When_I_Call_GetLastUpdatedDateTime_Then_The_Correct_Value_Is_Returned()
        {
            //arrange
            const string serverParkName = "LocalDevelopment";
            const string instrumentName = "OPN2102R";
            const string dateValue = "02-12-2021";
            const string timeValue = "09:23:59";

            var lastUpdated = DateTime.ParseExact($"{dateValue} {timeValue}", "dd-MM-yyyy hh:mm:ss", CultureInfo.InvariantCulture);

            var fieldData = new Dictionary<string, string>
            {
                {FieldNameType.HOut.FullName(), "110"},
                {FieldNameType.TelNo.FullName(), "07000000000"},
                {FieldNameType.LastUpdatedDate.FullName(), dateValue},
                {FieldNameType.LastUpdatedTime.FullName(), timeValue}
            };

            _sut.CreateCase(_primaryKey, fieldData, instrumentName, serverParkName);

            //act
            var dataRecord = _sut.GetCase(_primaryKey, instrumentName, serverParkName);

            var result = _sut.GetLastUpdated(dataRecord);

            //arrange
            Assert.AreEqual(lastUpdated, result);

            //cleanup
            _sut.RemoveCase(_primaryKey, instrumentName, serverParkName);
        }

        [Ignore("Integration")]
        [Test]
        public void Given_Value_Set_When_I_Call_GetField_Then_The_Correct_Value_Is_Returned()
        {
            //arrange
            const string serverParkName = "LocalDevelopment";
            const string instrumentName = "OPN2102R";

            var fieldData = new Dictionary<string, string>
            {
                {FieldNameType.HOut.FullName(), "110"},
                {FieldNameType.TelNo.FullName(), "07000000000"},
                {FieldNameType.CaseId.FullName(), _primaryKey}
            };

            _sut.CreateCase(_primaryKey, fieldData, instrumentName, serverParkName);

            //act
            var dataRecord = _sut.GetCase(_primaryKey, instrumentName, serverParkName);

            var result = _sut.GetFieldValue(dataRecord, FieldNameType.CaseId);

            //arrange
            Assert.AreEqual(_primaryKey, result);

            //cleanup
            _sut.RemoveCase(_primaryKey, instrumentName, serverParkName);
        }
    }
}
