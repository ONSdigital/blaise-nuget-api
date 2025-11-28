namespace Blaise.Nuget.Api.Tests.Behaviour.Case
{
    using System.Collections.Generic;
    using Blaise.Nuget.Api.Api;
    using Blaise.Nuget.Api.Contracts.Enums;
    using Blaise.Nuget.Api.Contracts.Extensions;
    using NUnit.Framework;

    public class UpdateCaseTests
    {
        private readonly BlaiseCaseApi _sut;

        private readonly Dictionary<string, string> _primaryKeyValues;

        public UpdateCaseTests()
        {
            _sut = new BlaiseCaseApi();
            _primaryKeyValues = new Dictionary<string, string> { { "QID.Serial_Number", "900001" } };
        }

        [Ignore("Integration")]
        [Test]
        public void When_I_Call_UpdateCase_Then_The_Case_Is_Updated()
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

            fieldData[FieldNameType.TelNo.FullName()] = "0711111111";

            var existingCase = _sut.GetCase(_primaryKeyValues, QuestionnaireName, ServerParkName);

            // act
            _sut.UpdateCase(existingCase, fieldData, QuestionnaireName, ServerParkName);

            existingCase = _sut.GetCase(_primaryKeyValues, QuestionnaireName, ServerParkName);
            var fields = _sut.GetRecordDataFields(existingCase);

            // assert
            Assert.That(fields[FieldNameType.TelNo.FullName()], Is.EqualTo("0711111111"));

            // cleanup
            _sut.RemoveCase(_primaryKeyValues, QuestionnaireName, ServerParkName);
        }

        [Ignore("Integration")]
        [Test]
        public void Given_An_Existing_Case_Is_Locked_When_I_Call_DataRecordIsLocked_Then_True_Is_Returned()
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
            var result = _sut.DataRecordIsLocked(_primaryKeyValues, QuestionnaireName, ServerParkName);

            // assert
            Assert.That(result, Is.True);

            // cleanup
            _sut.UnLockDataRecord(_primaryKeyValues, QuestionnaireName, ServerParkName, LockId);
            _sut.RemoveCase(_primaryKeyValues, QuestionnaireName, ServerParkName);
        }

        [Ignore("Integration")]
        [Test]
        public void Given_An_Existing_Case_Is_Not_Locked_When_I_Call_DataRecordIsLocked_Then_False_Is_Returned()
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
            var result = _sut.DataRecordIsLocked(_primaryKeyValues, QuestionnaireName, ServerParkName);

            // assert
            Assert.That(result, Is.False);

            // cleanup
            _sut.RemoveCase(_primaryKeyValues, QuestionnaireName, ServerParkName);
        }
    }
}
