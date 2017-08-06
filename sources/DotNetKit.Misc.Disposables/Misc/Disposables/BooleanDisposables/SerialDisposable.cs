using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetKit.Misc.Disposables
{
    /// <summary>
    /// Represents a boolean disposable to own up to one disposable.
    /// Whenever the content changed, disposes the old one.
    /// Thread-safe.
    /// </summary>
    public sealed class SerialDisposable
        : IBooleanDisposable
    {
        static readonly IDisposable disposed = new EmptyDisposable();

        IDisposable content;

        /// <summary>
        /// Gets a value indicating whether this is disposed.
        /// </summary>
        public bool IsDisposed => content == disposed;

        /// <summary>
        /// Sets the content.
        /// Returns <c>true</c> if set.
        /// </summary>
        bool TrySetContent(IDisposable disposable)
        {
            while (true)
            {
                var oldContent = content;
                if (oldContent == disposed)
                {
                    disposable.Dispose();
                    return false;
                }

                if (oldContent == disposable)
                {
                    return false;
                }

                var r = Interlocked.CompareExchange(ref content, disposable, oldContent);
                if (r == disposed)
                {
                    disposable.Dispose();
                    return false;
                }

                if (r == oldContent)
                {
                    oldContent.Dispose();
                    return true;
                }
            }
        }

        /// <summary>
        /// <para>
        /// Getter: Gets the content.
        /// </para>
        /// <para>
        /// Setter: Sets the content, disposing the current one.
        /// If this is disposed, disposes it instead.
        /// </para>
        /// </summary>
        /// <exception cref="ArgumentNullException" />
        public IDisposable Content
        {
            get
            {
                // Don't expose the marker.
                var d = Volatile.Read(ref content);
                return d == disposed ? EmptyDisposable.Instance : d;
            }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                TrySetContent(value);
            }
        }

        /// <summary>
        /// Tries to dispose the content.
        /// </summary>
        /// <returns></returns>
        public bool TryDispose()
        {
            return TrySetContent(disposed);
        }

        /// <summary>
        /// Disposes the content.
        /// </summary>
        public void Dispose()
        {
            TryDispose();
        }

        /// <summary>
        /// Contructs a serial disposable with the specified content.
        /// </summary>
        /// <exception cref="ArgumentNullException" />
        public SerialDisposable(IDisposable disposable)
        {
            content = disposable ?? throw new ArgumentNullException(nameof(disposable));
        }

        /// <summary>
        /// Constructs a serial disposable.
        /// The initial content is <see cref="EmptyDisposable.Instance"/>.
        /// </summary>
        /// <exception cref="ArgumentNullException" />
        public SerialDisposable()
            : this(EmptyDisposable.Instance)
        {
        }
    }
}
