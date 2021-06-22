using System;
using System.Collections.Generic;
using System.Globalization;
using Blaise.Nuget.Api.Api;
using Blaise.Nuget.Api.Contracts.Enums;
using Blaise.Nuget.Api.Contracts.Extensions;
using NUnit.Framework;

namespace Blaise.Nuget.Api.Tests.Behaviour.Case
{
    public class LiveDateTests
    {
        private readonly BlaiseCaseApi _sut;
        private readonly string _primaryKey;

        public LiveDateTests()
        {
            _sut = new BlaiseCaseApi();
            _primaryKey = "9000001";
        }

        [Ignore("Integration")]
        [Test]
        public void Given_A_Case_Exists_With_A_Live_DateWhen_I_Call_CaseExists_Then_True_Is_Returned()
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

            //assert
            Assert.AreEqual(liveDate, result);

            //cleanup
            _sut.RemoveCase(_primaryKey, instrumentName, serverParkName);
        }
    }
}
