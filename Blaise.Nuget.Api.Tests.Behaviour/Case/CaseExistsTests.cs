namespace Blaise.Nuget.Api.Tests.Behaviour.Case
{
    using System.Collections.Generic;
    using Blaise.Nuget.Api.Api;
    using Blaise.Nuget.Api.Contracts.Enums;
    using Blaise.Nuget.Api.Contracts.Extensions;
    using NUnit.Framework;

    public class CaseExistsTests
    {
        private readonly BlaiseCaseApi _sut;

        private readonly Dictionary<string, string> _primaryKeyValues;

        public CaseExistsTests()
        {
            _sut = new BlaiseCaseApi();
            _primaryKeyValues = new Dictionary<string, string> { { "QID.Serial_Number", "900001" } };
        }

        [Ignore("Integration")]
        [Test]
        public void Given_A_Case_Exists_When_I_Call_CaseExists_Then_True_Is_Returned()
        {
            // arrange
            const string ServerParkName = "LocalDevelopment";
            const string QuestionnaireName = "OPN2101A";

            var fieldData = new Dictionary<string, string>
            {
                { FieldNameType.HOut.FullName(), "110" },
                { FieldNameType.TelNo.FullName(), "07000000000" },
            };

            _sut.CreateCase(_primaryKeyValues, fieldData, QuestionnaireName, ServerParkName);

            // act
            var result = _sut.CaseExists(_primaryKeyValues, QuestionnaireName, ServerParkName);

            // assert
            Assert.That(result, Is.True);

            // cleanup
            _sut.RemoveCase(_primaryKeyValues, QuestionnaireName, ServerParkName);
        }

        [Ignore("Integration")]
        [Test]
        public void Given_A_Case_Does_Not_Exist_When_I_Call_CaseExists_Then_False_Is_Returned()
        {
            // arrange
            const string ServerParkName = "LocalDevelopment";
            const string QuestionnaireName = "OPN2101A";

            // act
            var result = _sut.CaseExists(_primaryKeyValues, QuestionnaireName, ServerParkName);

            // assert
            Assert.That(result, Is.False);
        }

        [Ignore("Integration")]
        [Test]
        public void Given_A_Record_Is_Locked_When_I_Call_CaseExists_Then_True_Is_Returned()
        {
            // arrange
            const string ServerParkName = "LocalDevelopment";
            const string QuestionnaireName = "OPN2101A";
            const string LockId = "Lock123";

            var fieldData = new Dictionary<string, string>
            {
                { FieldNameType.HOut.FullName(), "110" },
                { FieldNameType.TelNo.FullName(), "07000000000" },
            };

            _sut.CreateCase(_primaryKeyValues, fieldData, QuestionnaireName, ServerParkName);
            _sut.LockDataRecord(_primaryKeyValues, QuestionnaireName, ServerParkName, LockId);

            // act
            var result = _sut.CaseExists(_primaryKeyValues, QuestionnaireName, ServerParkName);

            // assert
            Assert.That(result, Is.True);

            // cleanup
            _sut.UnLockDataRecord(_primaryKeyValues, QuestionnaireName, ServerParkName, LockId);
            _sut.RemoveCase(_primaryKeyValues, QuestionnaireName, ServerParkName);
        }
    }
}
