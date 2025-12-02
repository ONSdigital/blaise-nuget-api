namespace Blaise.Nuget.Api.Tests.Unit.Contracts
{
    using System;
    using System.Collections.Generic;
    using Blaise.Nuget.Api.Contracts.Models;
    using NUnit.Framework;

    public class CaseModelTests
    {
        [Test]
        public void Given_A_Case_Model_Has_Populated_Primary_Keys_When_I_Call_GetPrimaryKeyValue_Then_The_Correct_Primary_Key_Value_Is_Returned()
        {
            // arrange
            var primaryKeyName = "QID.Serial_Number";
            var primaryKeyValue = "900001";
            var primaryKeyValues = new Dictionary<string, string> { { primaryKeyName, primaryKeyValue } };
            var caseModel = new CaseModel(primaryKeyValues, new Dictionary<string, string>());

            // act
            var result = caseModel.GetPrimaryKeyValue(primaryKeyName);

            // assert
            Assert.That(result, Is.EqualTo(primaryKeyValue));
        }

        [Test]
        public void Given_A_Case_Model_Does_Not_Have_Any_Primary_Keys_When_I_Call_GetPrimaryKeyValue_Then_An_ArgumentOutOfRangeException_Is_Thrown()
        {
            // arrange
            var primaryKeyName = "QID.Serial_Number";
            var caseModel = new CaseModel();

            // act and assert
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() => caseModel.GetPrimaryKeyValue(primaryKeyName));
            Assert.That(exception?.Message, Is.EqualTo("There are no primary keys defined\r\nParameter name: primaryKeyName"));
        }

        [Test]
        public void Given_A_Case_Model_Has_A_Case_Id_When_I_Access_The_Primary_Key_Property_Then_The_Correct_Value_Is_Returned()
        {
            // arrange
            var caseId = "900001";
            var primaryKeyValues = new Dictionary<string, string> { { "MainSurveyID", "dgss-5ghghg-ttggh" }, { "QID.Serial_Number", caseId } };
            var caseModel = new CaseModel(primaryKeyValues, new Dictionary<string, string>());

            // act
            var result = caseModel.PrimaryKey;

            // assert
            Assert.That(result, Is.EqualTo(caseId));
        }
    }
}
