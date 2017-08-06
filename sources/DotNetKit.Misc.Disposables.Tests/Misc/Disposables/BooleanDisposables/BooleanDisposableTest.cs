using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace DotNetKit.Misc.Disposables
{
    public sealed class BooleanDisposableTest
    {
        [Fact]
        public void Test_TryDispose()
        {
            var d = new BooleanDisposable();
            d.IsDisposed.Is(false);

            d.TryDispose().Is(true);
            d.IsDisposed.Is(true);

            d.TryDispose().Is(false);
            d.IsDisposed.Is(true);
        }

        [Fact]
        public void Test_it_gets_disposed_at_most_once()
        {
            foreach (var round in Enumerable.Range(0, 1000))
            {
                var d = new BooleanDisposable();
                BooleanDisposableTestHelper.TestTryDisposeExecuteAtMostOnce(d);
            }
        }
    }
}
