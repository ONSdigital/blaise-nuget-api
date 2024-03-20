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
            var result = _sut.GetCase(_primaryKeyValues,  questionnaireName, serverParkName);

            //arrange
            Assert.AreEqual(_primaryKeyValues, _sut.GetPrimaryKeyValues(result));

            //cleanup
            _sut.RemoveCase(_primaryKeyValues, questionnaireName, serverParkName);
        }
    }
}
