using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace DotNetKit.Misc.Disposables
{
    public sealed class TupleDisposableTest
    {
        [Fact]
        public void Test_TryDispose()
        {
            var d1 = new SensitiveDisposable();
            var d2 = new SensitiveDisposable();
            var d = ImmutableCompositeDisposable.Create(d1, d2);

            d.IsDisposed.Is(false);

            d.TryDispose().Is(true);
            d.IsDisposed.Is(true);
            d1.IsDisposed.Is(true);
            d2.IsDisposed.Is(true);

            d.TryDispose().Is(false);
        }

        [Fact]
        public void Test_TryDispose_concurrency()
        {
            foreach (var round in Enumerable.Range(0, 1000))
            {
                var d1 = new SensitiveDisposable();
                var d2 = new SensitiveDisposable();
                var d = ImmutableCompositeDisposable.Create(d1, d2);

                BooleanDisposableTestHelper.TestTryDisposeExecuteAtMostOnce(d);

                d1.IsDisposed.Is(true);
                d2.IsDisposed.Is(true);
            }
        }
    }
}
