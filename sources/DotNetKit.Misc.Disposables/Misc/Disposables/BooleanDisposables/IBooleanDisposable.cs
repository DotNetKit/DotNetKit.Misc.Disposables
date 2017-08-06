using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetKit.Misc.Disposables
{
    /// <summary>
    /// Represents a resource to be disposed.
    /// </summary>
    public interface IBooleanDisposable
        : IDisposable
    {
        /// <summary>
        /// Gets a value indicating whether this is disposed.
        /// </summary>
        bool IsDisposed { get; }

        /// <summary>
        /// Tries to dispose this.
        /// Returns <c>true</c> if disposed.
        /// </summary>
        bool TryDispose();
    }
}
