using System;
using System.Collections.Generic;
using System.Linq;

namespace NestedSets
{
    public static class Assert
    {
        public static void IsEmpty<T>(IEnumerable<T> items, string errorMessage)
            => ThrowIfTrue(items.Any(), errorMessage);

        public static void AreEqual<T>(T first, T second, string errorMessage)
            => ThrowIfTrue(!object.Equals(first, second), errorMessage);

        private static void ThrowIfTrue(bool test, string errorMessage)
        { if (test) throw new AssertionException(errorMessage); }
    }

    public class AssertionException : Exception
    {
        public AssertionException(string message) : base(message) { }
    }
}