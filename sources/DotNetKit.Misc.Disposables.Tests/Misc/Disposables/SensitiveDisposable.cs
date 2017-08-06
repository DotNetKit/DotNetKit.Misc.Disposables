using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetKit.Misc.Disposables
{
    /// <summary>
    /// Represents a disposable which throws an exception when disposed more than once.
    /// </summary>
    public sealed class SensitiveDisposable
        : IBooleanDisposable
    {
        int count;

        public bool IsDisposed => count > 0;

        public bool TryDispose()
        {
            if (Interlocked.Exchange(ref count, 1) == 0)
            {
                return true;
            }

            throw new InvalidOperationException("Disposed more than once.");
        }

        public void Dispose()
        {
            TryDispose();
        }
    }
}
