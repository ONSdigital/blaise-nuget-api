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
        private readonly string _primaryKey;

        public GetCaseTests()
        {
            _sut = new BlaiseCaseApi();
            _primaryKey = "9000001";
        }

        [Ignore("Integration")]
        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetCase_Then_The_Case_is_Returned()
        {
            //arrange
            const string serverParkName = "LocalDevelopment";
            const string instrumentName = "OPN2102R";
            var fieldData = new Dictionary<string, string>
            {
                {FieldNameType.HOut.FullName(), "110"},
                {FieldNameType.TelNo.FullName(), "07000000000"}
            };

            _sut.CreateCase(_primaryKey, fieldData, instrumentName, serverParkName);

            //act
            var result = _sut.GetCase(_primaryKey,  instrumentName, serverParkName);

            //arrange
            Assert.AreEqual(_primaryKey, _sut.GetPrimaryKeyValue(result));

            //cleanup
            _sut.RemoveCase(_primaryKey, instrumentName, serverParkName);
        }
    }
}
