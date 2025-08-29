using System.Collections.Generic;
using Blaise.Nuget.Api.Api;
using Blaise.Nuget.Api.Contracts.Enums;
using Blaise.Nuget.Api.Contracts.Extensions;
using NUnit.Framework;

namespace Blaise.Nuget.Api.Tests.Behaviour.Case
{
    public class DeleteCaseTests
    {
        private readonly BlaiseCaseApi _sut;

        public DeleteCaseTests()
        {
            _sut = new BlaiseCaseApi();
        }

        [Ignore("Integration")]
        [Test]
        public void Given_Valid_Arguments_When_I_Call_RemoveCases_Then_Then_All_Cases_Are_Deleted()
        {
            //arrange
            const string serverParkName = "LocalDevelopment";
            const string questionnaireName = "OPN2101A";
            var primaryKey = 9000001;

            var fieldData = new Dictionary<string, string>
            {
                {FieldNameType.HOut.FullName(), "110"},
                {FieldNameType.TelNo.FullName(), "07000000000"}
            };

            for (var i = 0; i < 1000; i++)
            {
                var primaryKeyValues = new Dictionary<string, string> { { "QID.Serial_Number", primaryKey.ToString() } };
                _sut.CreateCase(primaryKeyValues, fieldData, questionnaireName, serverParkName);
                primaryKey++;
            }

            //act
            _sut.RemoveCases(questionnaireName, serverParkName);
            var result = _sut.GetNumberOfCases(questionnaireName, serverParkName);

            //assert
            Assert.That(result, Is.EqualTo(0));
        }
    }
}
