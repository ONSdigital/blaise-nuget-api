using System;
using System.Collections.Generic;
using System.Globalization;
using Blaise.Nuget.Api.Api;
using Blaise.Nuget.Api.Contracts.Enums;
using Blaise.Nuget.Api.Contracts.Extensions;
using NUnit.Framework;

namespace Blaise.Nuget.Api.Tests.Behaviour.Field
{
    public class LiveDateFieldTests
    {
        private readonly BlaiseCaseApi _sut;
        private readonly string _primaryKey;

        public LiveDateFieldTests()
        {
            _sut = new BlaiseCaseApi();
            _primaryKey = "9000001";
        }

        [Ignore("Integration")]
        [Test]
        public void Given_A_Case_Exists_With_LiveDate_Set_When_I_Call_GetLiveDate_Then_The_Correct_Value_Is_Returned()
        {
            //arrange
            const string serverParkName = "LocalDevelopment";
            const string instrumentName = "LMS2102_BK1";
            var liveDate = DateTime.Today;

            var fieldData = new Dictionary<string, string>
            {
                {FieldNameType.HOut.FullName(), "110"},
                {FieldNameType.TelNo.FullName(), "07000000000"},
                {FieldNameType.LiveDate.FullName(), liveDate.ToString(CultureInfo.InvariantCulture)}
            };

            _sut.CreateCase(_primaryKey, fieldData, instrumentName, serverParkName);

            //act
            var result = _sut.GetLiveDate(instrumentName, serverParkName);

            //arrange
            Assert.AreEqual(liveDate, result);

            //cleanup
            _sut.RemoveCase(_primaryKey, instrumentName, serverParkName);
        }
        
        [Ignore("Integration")]
        [Test]
        public void Given_No_Cases_exist_When_I_Call_GetLiveDate_Then_Null_Returned()
        {
            //arrange
            const string serverParkName = "LocalDevelopment";
            const string instrumentName = "LMS2102_BK1";

            //act
            var result = _sut.GetLiveDate(instrumentName, serverParkName);

            //arrange
            Assert.IsNull(result);

            //cleanup
        }
    }
}
