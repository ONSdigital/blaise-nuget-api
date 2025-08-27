using System;
using Blaise.Nuget.Api.Extensions;
using NUnit.Framework;

namespace Blaise.Nuget.Api.Tests.Unit.Extensions
{
    public class ConfigurationExtensionsTests
    {
        [TestCase("30", 30)]
        public void Given_A_Valid_Argument_When_I_Call_GetVariableAsInt_I_Get_A_Correct_Value_Returned(string variable, int expectedResult)
        {
            //arrange
            const string variableName = "name";

            //act
            var result = ConfigurationExtensions.GetVariableAsInt(variable, variableName);

            //assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [TestCase("one")]
        [TestCase("")]
        public void Given_An_Invalid_Argument_When_I_Call_GetVariableAsInt_A_NullReferenceException_Is_Thrown(string invalidArgument)
        {
            //arrange
            const string variableName = "name";

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => ConfigurationExtensions.GetVariableAsInt(invalidArgument, variableName));
            Assert.That(exception.Message, Is.EqualTo($"A int value for the argument '{variableName}' must be supplied"));
        }
    }
}
