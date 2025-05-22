using System;
using System.Collections.Generic;

namespace Blaise.Nuget.Api.Core.Equality
{
    public class RemoteDataLinkKeyComparison : IEqualityComparer<Tuple<string, string, DateTime>>
    {
        public bool Equals(Tuple<string, string, DateTime> x, Tuple<string, string, DateTime> y)
        {
            if (x == null || y == null)
            {
                return false;
            }

            return StringComparer.CurrentCultureIgnoreCase.Equals(x.Item1, y.Item1) &&
                   StringComparer.CurrentCultureIgnoreCase.Equals(x.Item2, y.Item2) &&
                   x.Item3 == y.Item3;
        }

        public int GetHashCode(Tuple<string, string, DateTime> key)
        {
            return StringComparer.CurrentCultureIgnoreCase.GetHashCode(key.Item1) ^
                   StringComparer.CurrentCultureIgnoreCase.GetHashCode(key.Item2) ^
                   key.Item3.GetHashCode();

        }
    }
}
