using System;
using System.Collections.Generic;

namespace NestedSets
{
    public static class Extensions
    {
        public static void ForEach<T>(this IEnumerable<T> list, Action<T> action)
        {
            foreach (var elem in list)
                action(elem);
        }
    
        public static string ToStringWithPrepend<T>(this IEnumerable<T> list, string separator)
        {
            if (list == null)
                return string.Empty;
            var end = string.Join(separator, list);
            return string.IsNullOrEmpty(end)? end : separator + end;
        }
    }
}