
using System;

namespace Blaise.Nuget.Api.Core.Extensions
{
    public static class ExpiryExtensions
    {
        public static bool HasExpired(this DateTime dateTime)
        {
            return dateTime < DateTime.Now;
        }

        public static DateTime GetExpiryDate(this int expiryInMinutes)
        {
            return DateTime.Now.AddMinutes(expiryInMinutes);
        }
    }
}
