using System;
using System.Collections.Generic;
using System.Linq;

namespace Fluidity.Extensions
{
    internal static class EnumerableExtensions
    {
        public static IEnumerable<T> Flatten<T>(this IEnumerable<T> e, Func<T, IEnumerable<T>> f)
        {
            var enumerable = e as T[] ?? e.ToArray();
            return enumerable.SelectMany(c => f(c).Flatten(f)).Concat(enumerable);
        }
    }
}
