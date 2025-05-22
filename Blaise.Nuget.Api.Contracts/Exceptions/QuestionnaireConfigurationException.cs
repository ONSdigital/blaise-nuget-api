using System;

namespace Blaise.Nuget.Api.Contracts.Exceptions
{
    public class QuestionnaireConfigurationException : Exception
    {
        public QuestionnaireConfigurationException(string message) : base(message)
        {
        }
    }
}
