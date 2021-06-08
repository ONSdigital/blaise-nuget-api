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
        public void Given_Value_Set_When_I_Call_GetLiveDate_Then_The_Correct_Value_Is_Returned()
        {
            //arrange
            const string serverParkName = "gusty";
            const string instrumentName = "LMS2102_BK1";
            var liveDate = DateTime.Today;

            var fieldData = new Dictionary<string, string>
            {
                {FieldNameType.HOut.FullName(), "110"},
                {FieldNameType.TelNo.FullName(), "07000000000"},
                {FieldNameType.LiveDate.FullName(), liveDate.ToString(CultureInfo.InvariantCulture)},
            };

            _sut.CreateCase(_primaryKey, fieldData, instrumentName, serverParkName);

            //act
            var dataRecord = _sut.GetCase(_primaryKey, instrumentName, serverParkName);

            var result = _sut.GetLiveDate(dataRecord);

            //arrange
            Assert.AreEqual(liveDate, result);

            //cleanup
            _sut.RemoveCase(_primaryKey, instrumentName, serverParkName);
        }

        //[Ignore("Integration")]
        [Test]
        public void Given_I_Call_GetLiveDate_Then_The_Correct_Value_Is_Returned()
        {
            //arrange
            const string serverParkName = "LocalDevelopment";
            const string instrumentName = "LMS2102_BK1";
            var liveDate = DateTime.Today;

            //act

            var result = _sut.GetFieldValue("a", instrumentName, serverParkName, FieldNameType.LiveDate);

            //arrange
            Assert.AreEqual(liveDate, result);

            //cleanup
            _sut.RemoveCase(_primaryKey, instrumentName, serverParkName);
        }
    }
}
