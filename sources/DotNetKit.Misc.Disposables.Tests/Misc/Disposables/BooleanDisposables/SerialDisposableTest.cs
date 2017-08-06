using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DotNetKit.Misc.Disposables
{
    public sealed class SerialDisposableTest
    {
        readonly SerialDisposable it = new SerialDisposable();

        [Fact]
        public void Test_IsDisposed()
        {
            it.IsDisposed.Is(false);
            var d = new BooleanDisposable();
            it.Content = d;
            it.IsDisposed.Is(false);
            it.Dispose();
            it.IsDisposed.Is(true);
        }

        [Fact]
        public void Test_Content_get()
        {
            var d1 = new BooleanDisposable();
            var d2 = new BooleanDisposable();
            it.Content = d1;
            it.Content.Is(d1);
            it.Content = d2;
            it.Content.Is(d2);
            it.Dispose();
            it.Content.Is(EmptyDisposable.Instance);
        }

        [Fact]
        public void Test_it_disposes_old_ones()
        {
            var d1 = new BooleanDisposable();
            it.Content = d1;
            d1.IsDisposed.Is(false);

            var d2 = new BooleanDisposable();
            it.Content = d2;
            d1.IsDisposed.Is(true);
            d2.IsDisposed.Is(false);
        }

        [Fact]
        public void Test_it_does_not_dispose_current_one()
        {
            var d = new BooleanDisposable();
            it.Content = d;
            it.Content = d; // Assign again to make sure.
            d.IsDisposed.Is(false);
        }

        [Fact]
        public void Test_it_disposes_the_content_when_disposed()
        {
            var d = new BooleanDisposable();
            it.Content = d;
            it.Dispose();
            d.IsDisposed.Is(true);
        }

        [Fact]
        public void Test_it_disposes_new_ones_if_disposed()
        {
            it.Dispose();
            var d = new BooleanDisposable();
            it.Content = d;
            d.IsDisposed.Is(true);
        }

        [Fact]
        public void Test_it_is_safe_when_set_simultaneously()
        {
            var k = 10;

            foreach (var round in Enumerable.Range(0, 1000))
            {
                var ds =
                    Enumerable.Range(0, k)
                    .Select(i => new BooleanDisposable())
                    .ToArray();

                Parallel.ForEach(ds, d => it.Content = d);

                ds.Count(d => d.IsDisposed).Is(k - 1);
                it.Content.Is(ds.Single(d => !d.IsDisposed));
            }
        }

        [Fact]
        public void Test_it_is_safe_when_set_and_disposed_simultaneously()
        {
            var k = 10;

            foreach (var round in Enumerable.Range(0, 1000))
            {
                var ds =
                    Enumerable.Range(0, k)
                    .Select(i => new BooleanDisposable())
                    .ToArray();

                Parallel.Invoke(
                    () =>
                    {
                        Parallel.ForEach(ds, d => it.Content = d);
                    },
                    () => it.Dispose()
                );
                ds.All(d => d.IsDisposed).Is(true);
            }
        }
    }
}
