using System;

namespace Blaise.Nuget.Api.Core.Extensions
{
    public static class EnumExtensions
    {
        public static T ToEnum<T>(this string value) where T : struct
        {
            Enum.TryParse(value, true, out T status);
            return status;
        }
    }
}
