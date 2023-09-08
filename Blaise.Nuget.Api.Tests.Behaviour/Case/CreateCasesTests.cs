
using Blaise.Nuget.Api.Api;
using Blaise.Nuget.Api.Contracts.Enums;
using Blaise.Nuget.Api.Contracts.Extensions;
using Blaise.Nuget.Api.Contracts.Models;
using NUnit.Framework;
using System.Collections.Generic;
// ReSharper disable MissingXmlDoc

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
        {//here
            // Arrange
            const string serverParkName = "gusty";
            const string questionnaireName = "LMS2304_FS1";
            const int startingPrimaryKey = 90000;

            var fieldData
                = new Dictionary<string, string>
                                {
                                    {FieldNameType.HOut.FullName(), "110"},
                                    {FieldNameType.TelNo.FullName(), "07000000000"}
                                };


            var caseCount = 1000;
            var caseModels = GenerateCaseModels(startingPrimaryKey, caseCount, fieldData);

            // Act
            _sut.CreateCases(caseModels, questionnaireName, serverParkName);

            // Assert & Cleanup
            VerifyCasesExistAndRemove(startingPrimaryKey, caseCount, questionnaireName, serverParkName);
        }
        private List<CaseModel> GenerateCaseModels(int startingPrimaryKey, int caseCount, Dictionary<string, string> fieldData)
        {//here
            var caseModels = new List<CaseModel>();

            for (var loopCounter = 1; loopCounter <= caseCount; loopCounter++)
            {
                var caseId = $"{startingPrimaryKey + loopCounter}";
                caseModels.Add(new CaseModel(caseId, fieldData));
            }

            return caseModels;
        }

        private void VerifyCasesExistAndRemove(int startingPrimaryKey, int caseCount, string questionnaireName, string serverParkName)
        {//here
            for (var loopCounter = 1; loopCounter <= caseCount; loopCounter++)
            {
                var caseId = $"{startingPrimaryKey + loopCounter}";
                Assert.IsTrue(_sut.CaseExists(caseId, questionnaireName, serverParkName));
                _sut.RemoveCase(caseId, questionnaireName, serverParkName);
            }
        }
    }
}
