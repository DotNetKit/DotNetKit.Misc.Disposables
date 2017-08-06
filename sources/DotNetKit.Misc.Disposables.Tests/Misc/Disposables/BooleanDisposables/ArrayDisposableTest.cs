using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DotNetKit.Misc.Disposables
{
    public sealed class ArrayDisposableTest
    {
        [Fact]
        public void Test_variadic()
        {
            var d1 = new SensitiveDisposable();
            var d2 = new SensitiveDisposable();
            var d3 = new SensitiveDisposable();
            var d4 = new SensitiveDisposable();
            var d5 = new SensitiveDisposable();
            var d6 = new SensitiveDisposable();
            var it = ImmutableCompositeDisposable.Create(d1, d2, d3, d4, d5, d6);

            it.IsDisposed.Is(false);

            it.TryDispose().Is(true);
            new[] { d1, d2, d3, d4, d5, d6 }.All(d => d.IsDisposed).Is(true);
        }

        [Fact]
        public void Test_argument_verification()
        {
            Assert.Throws<ArgumentException>(() =>
                ImmutableCompositeDisposable.Create(
                    EmptyDisposable.Instance,
                    EmptyDisposable.Instance,
                    EmptyDisposable.Instance,
                    EmptyDisposable.Instance,
                    default(IDisposable)
                ));

            Assert.Throws<ArgumentException>(() =>
                ImmutableCompositeDisposable.FromEnumerable(new IDisposable[] { null })
            );
        }

        [Fact]
        public void Test_TryDisposable_executes_at_most_once()
        {
            foreach (var round in Enumerable.Range(0, 1000))
            {
                var ds = Enumerable.Range(0, 5).Select(_ => new SensitiveDisposable()).ToArray();

                BooleanDisposableTestHelper.TestTryDisposeExecuteAtMostOnce(
                    ImmutableCompositeDisposable.FromEnumerable(ds)
                );

                ds.All(d => d.IsDisposed).Is(true);
            }
        }
    }
}
