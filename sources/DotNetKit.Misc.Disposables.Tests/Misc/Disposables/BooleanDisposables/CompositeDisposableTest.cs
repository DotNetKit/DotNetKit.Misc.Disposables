using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace DotNetKit.Misc.Disposables
{
    public sealed class CompositeDisposableTest
    {
        [Fact]
        public void Test_it_disposes_all_contents_when_disposed()
        {
            var d1 = new BooleanDisposable();
            var d2 = new BooleanDisposable();
            var disposable = new CompositeDisposable();
            disposable.Add(d1);
            disposable.Add(d2);
            d1.IsDisposed.Is(false);
            d2.IsDisposed.Is(false);
            disposable.Dispose();
            d1.IsDisposed.Is(true);
            d2.IsDisposed.Is(true);
        }

        [Fact]
        public void Test_it_disposes_added_item_if_disposed()
        {
            var disposable = new CompositeDisposable();
            disposable.Dispose();

            var d = new BooleanDisposable();
            disposable.Add(d);
            d.IsDisposed.Is(true);
        }

        [Fact]
        public void Test_it_is_safe_to_add_simultaneously()
        {
            var k = 10;

            foreach (var round in Enumerable.Range(0, 1000))
            {
                var ds = Enumerable.Range(0, k).Select(i => new BooleanDisposable()).ToArray();
                var disposable = new CompositeDisposable();
                Parallel.ForEach(ds, d => disposable.Add(d));
                disposable.Dispose();
                ds.All(d => d.IsDisposed).Is(true);
            }
        }

        [Fact]
        public void Test_it_disposes_all_items()
        {
            var k = 10;

            foreach (var round in Enumerable.Range(0, 1000))
            {
                var disposable = new CompositeDisposable();
                var ds = Enumerable.Range(0, k).Select(i => new BooleanDisposable()).ToArray();

                Parallel.Invoke(
                    () => disposable.Dispose(),
                    () =>
                    {
                        foreach (var d in ds)
                        {
                            disposable.Add(d);
                        }
                    });
                ds.All(d => d.IsDisposed).Is(true);
            }
        }

        [Fact]
        public void Test_it_can_remove_items()
        {
            var disposable = new CompositeDisposable();
            var d = new BooleanDisposable();
            disposable.Add(d);
            disposable.Remove(d).Is(true);
            disposable.Dispose();
            d.IsDisposed.Is(false);
        }

        [Fact]
        public void Test_it_cannot_remove_nonexisting_item()
        {
            var disposable = new CompositeDisposable();
            disposable.Add(new BooleanDisposable());
            disposable.Remove(new BooleanDisposable()).Is(false);
        }

        [Fact]
        public void Test_it_is_safe_to_remove_simultaneously()
        {
            var k = 100;

            foreach (var round in Enumerable.Range(0, 1000))
            {
                var ds = Enumerable.Range(0, k).Select(i => new BooleanDisposable()).ToArray();
                var disposable = new CompositeDisposable();
                foreach (var d in ds)
                {
                    disposable.Add(d);
                }

                var removeCount = 0;
                Parallel.ForEach(ds, d =>
                {
                    var result = disposable.Remove(d);
                    if (result) Interlocked.Increment(ref removeCount);
                });

                disposable.Dispose();
                ds.All(d => !d.IsDisposed).Is(true);
                removeCount.Is(k);
            }
        }

        [Fact]
        public void Test_it_shrinks_when_sparsed()
        {
            var k = CompositeDisposable.ShrinkThreshold * 2;
            var disposable = new CompositeDisposable();

            var ds = Enumerable.Range(0, k).Select(_ => new BooleanDisposable()).ToArray();
            foreach (var d in ds)
            {
                disposable.Add(d);
            }
            (disposable.Capacity >= k).Is(true);

            foreach (var d in ds)
            {
                disposable.Remove(d);
            }
            (disposable.Capacity < k).Is(true);
        }
    }
}
