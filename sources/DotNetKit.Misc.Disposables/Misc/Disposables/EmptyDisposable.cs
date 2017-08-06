using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetKit.Misc.Disposables
{
    /// <summary>
    /// Represents a disposable which does nothing when disposed.
    /// </summary>
    public sealed class EmptyDisposable
        : IDisposable
    {
        /// <summary>
        /// Does nothing.
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// Gets an instance.
        /// </summary>
        public static EmptyDisposable Instance { get; } = new EmptyDisposable();

        /// <summary>
        /// Constructs a disposable which does nothing when disposed.
        /// Use <see cref="Instance"/> instead of creating new instance.
        /// </summary>
        public EmptyDisposable()
        {
        }
    }
}
