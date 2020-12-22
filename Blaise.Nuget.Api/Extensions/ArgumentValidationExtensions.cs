﻿using System;

namespace Blaise.Nuget.Api.Extensions
{
    internal static class ArgumentValidationExtensions
    {
        public static void ThrowExceptionIfNullOrEmpty(this string argument, string argumentName)
        {
            if (argument == null)
            {
                throw new ArgumentNullException(argumentName);
            }

            if (string.IsNullOrWhiteSpace(argument))
            {

                throw new ArgumentException($"A value for the argument '{argumentName}' must be supplied");
            }
        }

        public static void ThrowExceptionIfNull<T>(this T argument, string argumentName)
        {
            if (argument == null)
            {
                throw new ArgumentNullException($"The argument '{argumentName}' must be supplied");
            }
        }

        public static void ThrowExceptionIfNotInt(this string argument, string argumentName)
        {
            if (!int.TryParse(argument, out _))
            {
                throw new ArgumentException($"A int value for the argument '{argumentName}' must be supplied");
            }
        }
    }
}
