using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetKit.Misc.Disposables
{
    /// <summary>
    /// Represents an immutable array of disposables.
    /// Thread-safe.
    /// </summary>
    sealed class ArrayDisposable
        : IBooleanDisposable
    {
        static readonly IDisposable[] disposed = new IDisposable[0];

        IDisposable[] array;

        /// <summary>
        /// Gets a value indicating whether this is disposed.
        /// </summary>
        public bool IsDisposed => array == disposed;

        /// <summary>
        /// Tries to dispose all items.
        /// </summary>
        public bool TryDispose()
        {
            var disposables = Interlocked.Exchange(ref array, disposed);
            if (disposables == disposed) return false;

            foreach (var disposable in disposables)
            {
                disposable.Dispose();
            }
            return true;
        }

        /// <summary>
        /// Disposes all items.
        /// </summary>
        public void Dispose()
        {
            TryDispose();
        }

        public ArrayDisposable(IDisposable[] array)
        {
            this.array = array;
        }
    }
}
