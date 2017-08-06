using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DotNetKit
{
    public static class XunitExtension
    {
        public static void Is(this string actual, string expected)
        {
            Assert.Equal(expected, actual);
        }

        public static void Is<X>(this X actual, X expected)
        {
            Assert.Equal(expected, actual);
        }

        public static void IsSeq<X>(this IEnumerable<X> actual, IEnumerable<X> expected)
        {
            Assert.Equal(expected, actual);
        }
    }
}
