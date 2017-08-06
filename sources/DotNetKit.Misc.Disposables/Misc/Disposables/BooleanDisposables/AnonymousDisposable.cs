using System;
using System.Threading;

namespace DotNetKit.Misc.Disposables
{
    /// <summary>
    /// Represents a boolean disposable which invokes an action when disposed.
    /// Thread-safe.
    /// </summary>
    public sealed class AnonymousDisposable
        : IBooleanDisposable
    {
        static readonly Action disposed = () => { };

        Action dispose;

        /// <summary>
        /// Gets a value indicating whether this is disposed.
        /// </summary>
        public bool IsDisposed => dispose == disposed;

        /// <summary>
        /// Tries to dispose this.
        /// </summary>
        public bool TryDispose()
        {
            var action = Interlocked.Exchange(ref dispose, disposed);
            if (action == disposed) return false;
            action();
            return true;
        }

        /// <summary>
        /// Disposes this.
        /// </summary>
        public void Dispose()
        {
            TryDispose();
        }

        /// <summary>
        /// Contructs a disposable to execute the specified action when disposed.
        /// </summary>
        /// <exception cref="ArgumentNullException" />
        public AnonymousDisposable(Action dispose)
        {
            this.dispose = dispose ?? throw new ArgumentNullException(nameof(dispose));
        }
    }
}
