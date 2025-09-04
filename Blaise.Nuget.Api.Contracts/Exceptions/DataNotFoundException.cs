namespace Blaise.Nuget.Api.Contracts.Exceptions
{
    using System;

    public class DataNotFoundException : Exception
    {
        public DataNotFoundException(string message)
            : base(message)
        {
        }
    }
}
