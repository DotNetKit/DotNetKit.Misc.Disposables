
using System;
using System.Collections.Generic;
using System.Threading;

namespace DotNetKit.Misc.Disposables
{
    sealed class Tuple2Disposable
        : IBooleanDisposable
    {
        IDisposable disposable1, disposable2;

        public bool IsDisposed => disposable1 == null;

        public bool TryDispose()
        {
            var d = Interlocked.Exchange(ref disposable1, null);
            if (d == null) return false;
            d.Dispose();

            disposable2.Dispose();
            return true;
        }

        public void Dispose()
        {
            TryDispose();
        }

        public Tuple2Disposable(IDisposable disposable1, IDisposable disposable2)
        {
            this.disposable1 = disposable1;
            this.disposable2 = disposable2;
        }
    }

    public partial class ImmutableCompositeDisposable
    {
        /// <summary>
        /// Creates a composite disposable to own the specified disposables.
        /// </summary>
        /// <exception cref="ArgumentNullException" />
        public static IBooleanDisposable Create(IDisposable disposable1, IDisposable disposable2)
        {
            if (disposable1 == null) throw new ArgumentNullException(nameof(disposable1));
            if (disposable2 == null) throw new ArgumentNullException(nameof(disposable2));
            return new Tuple2Disposable(disposable1, disposable2);
        }
    }

    sealed class Tuple3Disposable
        : IBooleanDisposable
    {
        IDisposable disposable1, disposable2, disposable3;

        public bool IsDisposed => disposable1 == null;

        public bool TryDispose()
        {
            var d = Interlocked.Exchange(ref disposable1, null);
            if (d == null) return false;
            d.Dispose();

            disposable2.Dispose();
            disposable3.Dispose();
            return true;
        }

        public void Dispose()
        {
            TryDispose();
        }

        public Tuple3Disposable(IDisposable disposable1, IDisposable disposable2, IDisposable disposable3)
        {
            this.disposable1 = disposable1;
            this.disposable2 = disposable2;
            this.disposable3 = disposable3;
        }
    }

    public partial class ImmutableCompositeDisposable
    {
        /// <summary>
        /// Creates a composite disposable to own the specified disposables.
        /// </summary>
        /// <exception cref="ArgumentNullException" />
        public static IBooleanDisposable Create(IDisposable disposable1, IDisposable disposable2, IDisposable disposable3)
        {
            if (disposable1 == null) throw new ArgumentNullException(nameof(disposable1));
            if (disposable2 == null) throw new ArgumentNullException(nameof(disposable2));
            if (disposable3 == null) throw new ArgumentNullException(nameof(disposable3));
            return new Tuple3Disposable(disposable1, disposable2, disposable3);
        }
    }

    sealed class Tuple4Disposable
        : IBooleanDisposable
    {
        IDisposable disposable1, disposable2, disposable3, disposable4;

        public bool IsDisposed => disposable1 == null;

        public bool TryDispose()
        {
            var d = Interlocked.Exchange(ref disposable1, null);
            if (d == null) return false;
            d.Dispose();

            disposable2.Dispose();
            disposable3.Dispose();
            disposable4.Dispose();
            return true;
        }

        public void Dispose()
        {
            TryDispose();
        }

        public Tuple4Disposable(IDisposable disposable1, IDisposable disposable2, IDisposable disposable3, IDisposable disposable4)
        {
            this.disposable1 = disposable1;
            this.disposable2 = disposable2;
            this.disposable3 = disposable3;
            this.disposable4 = disposable4;
        }
    }

    public partial class ImmutableCompositeDisposable
    {
        /// <summary>
        /// Creates a composite disposable to own the specified disposables.
        /// </summary>
        /// <exception cref="ArgumentNullException" />
        public static IBooleanDisposable Create(IDisposable disposable1, IDisposable disposable2, IDisposable disposable3, IDisposable disposable4)
        {
            if (disposable1 == null) throw new ArgumentNullException(nameof(disposable1));
            if (disposable2 == null) throw new ArgumentNullException(nameof(disposable2));
            if (disposable3 == null) throw new ArgumentNullException(nameof(disposable3));
            if (disposable4 == null) throw new ArgumentNullException(nameof(disposable4));
            return new Tuple4Disposable(disposable1, disposable2, disposable3, disposable4);
        }
    }

}
