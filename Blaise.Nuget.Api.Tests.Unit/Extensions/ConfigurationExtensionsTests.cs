namespace Blaise.Nuget.Api.Tests.Unit.Extensions
{
    using System;
    using Blaise.Nuget.Api.Extensions;
    using NUnit.Framework;

    public class ConfigurationExtensionsTests
    {
        [TestCase("30", 30)]
        public void When_I_Call_GetVariableAsInt_Then_A_Correct_Value_Is_Returned(string variable, int expectedResult)
        {
            // arrange
            const string VariableName = "name";

            // act
            var result = ConfigurationExtensions.GetVariableAsInt(variable, VariableName);

            // assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [TestCase("one")]
        [TestCase("")]
        public void Given_An_Invalid_Argument_When_I_Call_GetVariableAsInt_Then_An_ArgumentException_Is_Thrown(string invalidArgument)
        {
            // arrange
            const string VariableName = "name";

            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => ConfigurationExtensions.GetVariableAsInt(invalidArgument, VariableName));
            Assert.That(exception.Message, Is.EqualTo($"A int value for the argument '{VariableName}' must be supplied"));
        }
    }
}
