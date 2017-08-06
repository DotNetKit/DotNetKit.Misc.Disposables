using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetKit.Misc.Disposables
{
    public static class BooleanDisposableTestHelper
    {
        public static void TestTryDisposeExecuteAtMostOnce(IBooleanDisposable d)
        {
            d.IsDisposed.Is(false);

            var count = 0;

            Parallel.For(0, 100, i =>
            {
                if (d.TryDispose())
                {
                    Interlocked.Increment(ref count);
                }
            });

            count.Is(1);
            d.IsDisposed.Is(true);
        }
    }
}
