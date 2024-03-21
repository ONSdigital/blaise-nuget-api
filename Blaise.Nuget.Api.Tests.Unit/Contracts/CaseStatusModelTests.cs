using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Globalization;
using Blaise.Nuget.Api.Contracts.Models;

namespace Blaise.Nuget.Api.Tests.Unit.Contracts
{
    public class CaseStatusModelTests
    {
        [Test]
        public void Given_A_CaseStatusModel_Has_Populated_PrimaryKeys_When_I_Call_GetPrimaryKeyValue_The_Correct_PrimaryKey_Value_Is_Returned()
        {
            // arrange
            var primaryKeyName = "QID.Serial_Number";
            var primaryKeyValue = "900001";
            var primaryKeyValues = new Dictionary<string, string> { { primaryKeyName, primaryKeyValue } };
            var caseStatusModel = new CaseStatusModel(primaryKeyValues, 110, DateTime.Now.ToString(CultureInfo.InvariantCulture));

            // act
            var result = caseStatusModel.GetPrimaryKeyValue(primaryKeyName);

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(primaryKeyValue, result);
        }

        [Test]
        public void Given_A_CaseStatusModel_Does_Not_Have_Any_PrimaryKeys_When_I_Call_GetPrimaryKeyValue_Then_An_ArgumentOutOfRangeException_Is_Thrown()
        {
            // arrange
            var primaryKeyName = "QID.Serial_Number";
            var caseStatusModel = new CaseStatusModel();

            //act && assert
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() => caseStatusModel.GetPrimaryKeyValue(primaryKeyName));
            Assert.AreEqual("There are no primary keys defined\r\nParameter name: primaryKeyName", exception?.Message); ;
        }
    }
}
