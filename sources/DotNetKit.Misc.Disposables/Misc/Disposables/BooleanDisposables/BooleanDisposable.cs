using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetKit.Misc.Disposables
{
    /// <summary>
    /// Represents a disposable which indicates whether this is disposed.
    /// Thread-safe.
    /// </summary>
    public sealed class BooleanDisposable
        : IBooleanDisposable
    {
        const int InitialState = 0;
        const int DisposedState = 1;

        int state = InitialState;

        /// <summary>
        /// Gets a value indicating whether this is disposed.
        /// </summary>
        public bool IsDisposed => state == DisposedState;

        /// <summary>
        /// Tries to dispose this.
        /// </summary>
        public bool TryDispose()
        {
            var original = Interlocked.Exchange(ref state, DisposedState);
            return original == InitialState;
        }

        /// <summary>
        /// Disposes this.
        /// </summary>
        public void Dispose()
        {
            TryDispose();
        }
    }
}
