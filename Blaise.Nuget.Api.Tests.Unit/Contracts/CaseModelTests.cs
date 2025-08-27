using System;
using NUnit.Framework;
using System.Collections.Generic;
using Blaise.Nuget.Api.Contracts.Models;

namespace Blaise.Nuget.Api.Tests.Unit.Contracts
{
    public class CaseModelTests
    {
        [Test]
        public void Given_A_CaseModel_Has_Populated_PrimaryKeys_When_I_Call_GetPrimaryKeyValue_The_Correct_PrimaryKey_Value_Is_Returned()
        {
            // arrange
            var primaryKeyName = "QID.Serial_Number";
            var primaryKeyValue = "900001";
            var primaryKeyValues = new Dictionary<string, string> { { primaryKeyName, primaryKeyValue } };
            var caseModel = new CaseModel(primaryKeyValues, new Dictionary<string, string>());

            // act
            var result = caseModel.GetPrimaryKeyValue(primaryKeyName);

            // assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(primaryKeyValue));
        }

        [Test]
        public void Given_A_CaseModel_Does_Not_Have_Any_PrimaryKeys_When_I_Call_GetPrimaryKeyValue_Then_An_ArgumentOutOfRangeException_Is_Thrown()
        {
            // arrange
            var primaryKeyName = "QID.Serial_Number";
            var caseModel = new CaseModel();

            //act && assert
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() => caseModel.GetPrimaryKeyValue(primaryKeyName));
            Assert.That(exception?.Message, Is.EqualTo("There are no primary keys defined\r\nParameter name: primaryKeyName"));
        }

        [Test]
        public void Given_A_CaseModel_Has_A_CaseId_When_I_Call_CaseId_The_Correct_PrimaryKey_Value_Is_Returned()
        {
            // arrange
            var caseId = "900001";
            var primaryKeyValues = new Dictionary<string, string> { { "MainSurveyID", "dgss-5ghghg-ttggh" }, { "QID.Serial_Number", caseId } };
            var caseStatusModel = new CaseModel(primaryKeyValues, new Dictionary<string, string>());

            // act
            var result = caseStatusModel.PrimaryKey;

            // assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(caseId));
        }
    }
}
