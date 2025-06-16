using System.Collections.Generic;
using Blaise.Nuget.Api.Api;
using Blaise.Nuget.Api.Contracts.Enums;
using Blaise.Nuget.Api.Contracts.Extensions;
using NUnit.Framework;

namespace Blaise.Nuget.Api.Tests.Behaviour.Case
{
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
        public void Given_Valid_Arguments_When_I_Call_GetCase_Then_The_Case_is_Returned()
        {
            //arrange
            const string serverParkName = "LocalDevelopment";
            const string questionnaireName = "OPN2102R";
            var fieldData = new Dictionary<string, string>
            {
                {FieldNameType.HOut.FullName(), "110"},
                {FieldNameType.TelNo.FullName(), "07000000000"}
            };

            _sut.CreateCase(_primaryKeyValues, fieldData, questionnaireName, serverParkName);

            //act
            var result = _sut.GetCase(_primaryKeyValues, questionnaireName, serverParkName);

            //arrange
            Assert.AreEqual(_primaryKeyValues, _sut.GetPrimaryKeyValues(result));

            //cleanup
            _sut.RemoveCase(_primaryKeyValues, questionnaireName, serverParkName);
        }

        [Ignore("Integration")]
        [Test]
        public void Given_Cases_Exist_When_I_Specify_A_Filter_Then_The_Expected_Cases_Are_Returned()
        {
            //arrange
            const string serverParkName = "gusty";
            const string questionnaireName = "LMS2405_HU1";
            const string filter = "Id=10";

            //act
            var result = _sut.GetFilteredCases(questionnaireName, serverParkName, filter);

            while (!result.EndOfSet)
            {
                var record = result.ActiveRecord;
                result.MoveNext();
            }

            //assert
            Assert.AreEqual(5, result.RecordCount);
        }
    }
}
