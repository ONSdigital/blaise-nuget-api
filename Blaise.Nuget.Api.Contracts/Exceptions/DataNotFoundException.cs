using System;

namespace Blaise.Nuget.Api.Contracts.Exceptions
{
    public class DataNotFoundException : Exception
    {
        public DataNotFoundException(string message) : base(message)
        {
        }
    }
}
