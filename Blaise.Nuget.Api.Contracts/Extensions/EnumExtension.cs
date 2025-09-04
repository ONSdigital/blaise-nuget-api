namespace Blaise.Nuget.Api.Contracts.Extensions
{
    using System;
    using System.ComponentModel;
    using System.Linq;

    public static class EnumExtension
    {
        public static string FullName(this Enum value)
        {
            var fi = value.GetType().GetField(value.ToString());

            if (fi.GetCustomAttributes(typeof(DescriptionAttribute), false) is DescriptionAttribute[] attributes && attributes.Any())
            {
                return attributes.First().Description;
            }

            return value.ToString();
        }
    }
}
