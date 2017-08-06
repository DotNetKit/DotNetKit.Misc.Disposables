using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DotNetKit.Misc.Disposables
{
    public sealed class SingleDisposableTest
    {
        [Fact]
        public void Test_IsEmpty()
        {
            var d = new SingleDisposable();
            d.IsEmpty.Is(true);

            d.Content = new BooleanDisposable();
            d.IsEmpty.Is(false);

            d.Dispose();
            d.IsEmpty.Is(false);
        }

        [Fact]
        public void Test_IsDisposed()
        {
            var d = new SingleDisposable();
            d.IsDisposed.Is(false);

            d.Content = new BooleanDisposable();
            d.IsDisposed.Is(false);

            d.Dispose();
            d.IsDisposed.Is(true);
        }

        [Fact]
        public void Test_Content_setter()
        {
            var d = new SingleDisposable();
            var d1 = new BooleanDisposable();
            var d2 = new BooleanDisposable();

            // The first content doesn't get disposed.
            d.Content = d1;
            d1.IsDisposed.Is(false);

            // Second content gets disposed.
            d.Content = d2;
            d.IsDisposed.Is(false);
            d1.IsDisposed.Is(false);
            d2.IsDisposed.Is(true);
        }

        [Fact]
        public void Test_Content_setter_when_disposed()
        {
            var d = new SingleDisposable();
            d.Dispose();

            // New value gets disposed if the container disposed.
            var d1 = new BooleanDisposable();
            d.Content = d1;
            d1.IsDisposed.Is(true);
        }

        [Fact]
        public void Test_Content_getter()
        {
            var d = new SingleDisposable();
            var d1 = new BooleanDisposable();
            var d2 = new BooleanDisposable();

            // The initial content is empty.
            d.Content.Is(EmptyDisposable.Instance);

            // Get the current content.
            d.Content = d1;
            d.Content.Is(d1);

            // Content doesn't change if set.
            d.Content = new BooleanDisposable();
            d.Content.Is(d1);

            // After disposed, the content becomes empty.
            d.Dispose();
            d.Content.Is(EmptyDisposable.Instance);
        }

        [Fact]
        public void Test_Content_setter_concurrency()
        {
            const int k = 10;

            foreach (var round in Enumerable.Range(0, 100))
            {
                var container = new SingleDisposable();
                var ds = Enumerable.Range(0, k).Select(_ => new BooleanDisposable()).ToArray();

                Parallel.ForEach(ds, d => container.Content = d);

                ds.Where(d => !d.IsDisposed).IsSeq(new[] { (BooleanDisposable)container.Content });
            }
        }

        [Fact]
        public void Test_TryDispose_disposes_content()
        {
            var d1 = new BooleanDisposable();
            var d = new SingleDisposable(d1);

            d.TryDispose().Is(true);
            d1.IsDisposed.Is(true);

            d.TryDispose().Is(false);
        }

        [Fact]
        public void Test_Dispose_concurrency()
        {
            foreach (var round in Enumerable.Range(0, 100))
            {
                var d = new SingleDisposable();
                var d1 = new BooleanDisposable();

                // When it gets the content and disposed simultaneously,
                // both it and the content get disposed.
                Parallel.Invoke(
                    () => d.Content = d1,
                    () => d.Dispose()
                );

                d.IsDisposed.Is(true);
                d1.IsDisposed.Is(true);
            }
        }
    }
}
