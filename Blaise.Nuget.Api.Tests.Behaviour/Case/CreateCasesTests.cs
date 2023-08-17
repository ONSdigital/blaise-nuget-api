
using System.Collections.Generic;
using Blaise.Nuget.Api.Api;
using Blaise.Nuget.Api.Contracts.Enums;
using Blaise.Nuget.Api.Contracts.Extensions;
using Blaise.Nuget.Api.Contracts.Models;
using NUnit.Framework;

namespace Blaise.Nuget.Api.Tests.Behaviour.Case
{
    public class CreateCasesTests
    {
        private readonly BlaiseCaseApi _sut;

        public CreateCasesTests()
        {
            _sut = new BlaiseCaseApi();
        }

        [Ignore("Integration")]
        [Test]
        public void Given_Valid_Arguments_When_I_Call_CreateCases_Then_The_Cases_Are_Created()
        {
            //arrange
            const string serverParkName = "gusty";
            const string questionnaireName = "LMS2304_FS1";

            var primaryKey = 90000;
            var fieldData = new Dictionary<string, string>
            {
                {FieldNameType.HOut.FullName(), "110"},
                {FieldNameType.TelNo.FullName(), "07000000000"}
            };

            var caseModels = new List<CaseModel>();

            for (var i = 1; i <= 1000; i++)
            {
                caseModels.Add(new CaseModel($"{primaryKey+i}", fieldData));
            }

            //act
            _sut.CreateCases(caseModels, questionnaireName, serverParkName);

            //assert && cleanup
            for (var i = 1; i <= 1000; i++)
            {
                var caseId = $"{primaryKey + i}";
                Assert.IsTrue(_sut.CaseExists(caseId, questionnaireName, serverParkName));
                _sut.RemoveCase(caseId, questionnaireName, serverParkName);
            }
        }
    }
}
