namespace Blaise.Nuget.Api.Tests.Unit.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Blaise.Nuget.Api.Contracts.Models;
    using NUnit.Framework;

    public class CaseStatusModelTests
    {
        [Test]
        public void Given_A_Case_Status_Model_Has_Populated_Primary_Keys_When_I_Call_GetPrimaryKeyValue_Then_The_Correct_Primary_Key_Value_Is_Returned()
        {
            // arrange
            var primaryKeyName = "QID.Serial_Number";
            var primaryKeyValue = "900001";
            var primaryKeyValues = new Dictionary<string, string> { { primaryKeyName, primaryKeyValue } };
            var caseStatusModel = new CaseStatusModel(primaryKeyValues, 110, DateTime.Now.ToString(CultureInfo.InvariantCulture));

            // act
            var result = caseStatusModel.GetPrimaryKeyValue(primaryKeyName);

            // assert
            Assert.That(result, Is.EqualTo(primaryKeyValue));
        }

        [Test]
        public void Given_A_Case_Status_Model_Has_Populated_Primary_Keys_When_I_Access_The_Primary_Key_Property_Then_The_Correct_Value_Is_Returned()
        {
            // arrange
            var primaryKeyName = "QID.Serial_Number";
            var primaryKeyValue = "900001";
            var primaryKeyValues = new Dictionary<string, string> { { primaryKeyName, primaryKeyValue } };
            var caseStatusModel = new CaseStatusModel(primaryKeyValues, 110, DateTime.Now.ToString(CultureInfo.InvariantCulture));

            // act
            var result = caseStatusModel.PrimaryKey;

            // assert
            Assert.That(result, Is.EqualTo(primaryKeyValue));
        }

        [Test]
        public void Given_A_Case_Status_Model_Does_Not_Have_Any_Primary_Keys_When_I_Call_GetPrimaryKeyValue_Then_An_ArgumentOutOfRangeException_Is_Thrown()
        {
            // arrange
            var primaryKeyName = "QID.Serial_Number";
            var caseStatusModel = new CaseStatusModel();

            // act and assert
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() => caseStatusModel.GetPrimaryKeyValue(primaryKeyName));
            Assert.That(exception?.Message, Is.EqualTo("There are no primary keys defined\r\nParameter name: primaryKeyName"));
        }

        [Test]
        public void Given_A_Case_Status_Model_Does_Not_Have_The_Expected_Primary_Keys_When_I_Call_GetPrimaryKeyValue_Then_An_ArgumentException_Is_Thrown()
        {
            // arrange
            var primaryKeyName = "QID.Serial_Number";
            var primaryKeyValues = new Dictionary<string, string> { { "MainSurveyID", "dgss-5ghghg-ttggh" } };
            var caseStatusModel = new CaseStatusModel(primaryKeyValues, 110, DateTime.Now.ToString(CultureInfo.InvariantCulture));

            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => caseStatusModel.GetPrimaryKeyValue(primaryKeyName));
            Assert.That(exception?.Message, Is.EqualTo($"The primary key name '{primaryKeyName}' does not exist in the primaryKey object"));
        }

        [Test]
        public void Given_A_Case_Status_Model_Has_A_Case_Id_When_I_Access_The_Primary_Key_Property_Then_The_Correct_Value_Is_Returned()
        {
            // arrange
            var caseId = "900001";
            var primaryKeyValues = new Dictionary<string, string> { { "MainSurveyID", "dgss-5ghghg-ttggh" }, { "QID.Serial_Number", caseId } };
            var caseStatusModel = new CaseStatusModel(primaryKeyValues, 110, DateTime.Now.ToString(CultureInfo.InvariantCulture));

            // act
            var result = caseStatusModel.PrimaryKey;

            // assert
            Assert.That(result, Is.EqualTo(caseId));
        }
    }
}
