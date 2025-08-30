namespace Blaise.Nuget.Api.Contracts.Exceptions
{
    using System;

    public class QuestionnaireConfigurationException : Exception
    {
        public QuestionnaireConfigurationException(string message)
            : base(message)
        {
        }
    }
}
