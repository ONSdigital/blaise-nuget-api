using System.Collections.Generic;
using Blaise.Nuget.Api.Api;
using Blaise.Nuget.Api.Contracts.Enums;
using Blaise.Nuget.Api.Contracts.Extensions;
using NUnit.Framework;

namespace Blaise.Nuget.Api.Tests.Behaviour.Case
{
    public class CaseExistsTests
    {
        private readonly BlaiseCaseApi _sut;
        private readonly string _primaryKey;

        public CaseExistsTests()
        {
            _sut = new BlaiseCaseApi();
            _primaryKey = "9000001";
        }

        [Ignore("Integration")]
        [Test]
        public void Given_A_Case_Exists_When_I_Call_CaseExists_Then_True_Is_Returned()
        {
            //arrange
            const string serverParkName = "LocalDevelopment";
            const string questionnaireName = "OPN2101A";

            var fieldData = new Dictionary<string, string>
            {
                {FieldNameType.HOut.FullName(), "110"},
                {FieldNameType.TelNo.FullName(), "07000000000"}
            };

            _sut.CreateCase(_primaryKey, fieldData, questionnaireName, serverParkName);

            //act
            var result = _sut.CaseExists(_primaryKey, questionnaireName, serverParkName);

            //assert
            Assert.IsTrue(result);

            //cleanup
            _sut.RemoveCase(_primaryKey, questionnaireName, serverParkName);
        }

        [Ignore("Integration")]
        [Test]
        public void Given_A_Case_Does_Not_Exist_When_I_Call_CaseExists_Then_True_Is_Returned()
        {
            //arrange
            const string serverParkName = "LocalDevelopment";
            const string questionnaireName = "OPN2101A";

            //act
            var result = _sut.CaseExists(_primaryKey, questionnaireName, serverParkName);

            //assert
            Assert.IsFalse(result);
        }

        [Ignore("Integration")]
        [Test]
        public void Given_A_Record_Is_Locked_When_I_Call_CaseExists_Then_True_Is_Returned()
        {
            //arrange
            const string serverParkName = "LocalDevelopment";
            const string questionnaireName = "OPN2101A";
            const string lockId = "Lock123";

            var fieldData = new Dictionary<string, string>
            {
                {FieldNameType.HOut.FullName(), "110"},
                {FieldNameType.TelNo.FullName(), "07000000000"}
            };

            _sut.CreateCase(_primaryKey, fieldData, questionnaireName, serverParkName);
            _sut.LockDataRecord(_primaryKey, questionnaireName, serverParkName, lockId);

            //act
            var result = _sut.CaseExists(_primaryKey, questionnaireName, serverParkName);

            //assert
            Assert.IsTrue(result);

            //cleanup
            _sut.UnLockDataRecord(_primaryKey, questionnaireName, serverParkName, lockId);
            _sut.RemoveCase(_primaryKey, questionnaireName, serverParkName);
        }
    }
}
