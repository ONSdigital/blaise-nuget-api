namespace Blaise.Nuget.Api.Tests.Behaviour.Case
{
    using System.Collections.Generic;
    using Blaise.Nuget.Api.Api;
    using Blaise.Nuget.Api.Contracts.Enums;
    using Blaise.Nuget.Api.Contracts.Extensions;
    using Blaise.Nuget.Api.Contracts.Models;
    using NUnit.Framework;

    public class CreateCasesTests
    {
        private readonly BlaiseCaseApi _sut;

        public CreateCasesTests()
        {
            _sut = new BlaiseCaseApi();
        }

        [Ignore("Integration")]
        [Test]
        public void When_I_Call_CreateCases_Then_The_Cases_Are_Created()
        {
            // arrange
            const string ServerParkName = "gusty";
            const string QuestionnaireName = "LMS2304_FS1";
            const int StartingPrimaryKey = 90000;

            var fieldData
                = new Dictionary<string, string>
                                {
                                    { FieldNameType.HOut.FullName(), "110" },
                                    { FieldNameType.TelNo.FullName(), "07000000000" },
                                };

            var caseCount = 1000;
            var caseModels = GenerateCaseModels(StartingPrimaryKey, caseCount, fieldData);

            // act
            _sut.CreateCases(caseModels, QuestionnaireName, ServerParkName);

            // assert
            VerifyCasesExistAndRemove(StartingPrimaryKey, caseCount, QuestionnaireName, ServerParkName);
        }

        private List<CaseModel> GenerateCaseModels(int startingPrimaryKey, int caseCount, Dictionary<string, string> fieldData)
        {
            var caseModels = new List<CaseModel>();

            for (var loopCounter = 1; loopCounter <= caseCount; loopCounter++)
            {
                var caseId = $"{startingPrimaryKey + loopCounter}";
                var primaryKeyValues = new Dictionary<string, string> { { "QID.Serial_Number", caseId } };
                caseModels.Add(new CaseModel(primaryKeyValues, fieldData));
            }

            return caseModels;
        }

        private void VerifyCasesExistAndRemove(int startingPrimaryKey, int caseCount, string questionnaireName, string serverParkName)
        {
            for (var loopCounter = 1; loopCounter <= caseCount; loopCounter++)
            {
                var caseId = $"{startingPrimaryKey + loopCounter}";
                var primaryKeyValues = new Dictionary<string, string> { { "QID.Serial_Number", caseId } };
                Assert.That(_sut.CaseExists(primaryKeyValues, questionnaireName, serverParkName), Is.True);
                _sut.RemoveCase(primaryKeyValues, questionnaireName, serverParkName);
            }
        }
    }
}
