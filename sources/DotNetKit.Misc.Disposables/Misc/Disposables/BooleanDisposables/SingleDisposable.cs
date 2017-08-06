using System;
using System.Threading;

namespace DotNetKit.Misc.Disposables
{
    /// <summary>
    /// Represents a boolean disposable to own up to one disposable.
    /// Thread-safe.
    /// </summary>
    public sealed class SingleDisposable
        : IBooleanDisposable
    {
        /// <summary>
        /// Value type version of <see cref="SingleDisposable" />.
        /// Don't copy.
        /// </summary>
        public struct Struct
            : IBooleanDisposable
        {
            const IDisposable empty = null;

            static readonly IDisposable disposed = new EmptyDisposable();

            IDisposable content;

            /// <summary>
            /// Gets a value indicating whether this is empty.
            /// </summary>
            public bool IsEmpty =>
                content == empty;

            /// <summary>
            /// Gets a value indicating whether this is disposed.
            /// </summary>
            public bool IsDisposed =>
                content == disposed;

            /// <summary>
            /// Tries to dispose the content.
            /// Returns <c>true</c> if disposed.
            /// </summary>
            public bool TryDispose()
            {
                var disposable = Interlocked.Exchange(ref content, disposed);
                if (disposable == disposed)
                {
                    return false;
                }

                disposable?.Dispose();
                return true;
            }

            /// <summary>
            /// Disposes the content.
            /// </summary>
            public void Dispose()
            {
                TryDispose();
            }

            /// <summary>
            /// <para>
            /// Getter: Gets the content.
            /// </para>
            /// <para>
            /// Setter: Sets the content.
            /// If this has content or is disposed, disposes the value instead.
            /// </para>
            /// </summary>
            /// <exception cref="ArgumentNullException" />
            public IDisposable Content
            {
                get
                {
                    // Don't expose markers.
                    if (content == empty || content == disposed)
                    {
                        return EmptyDisposable.Instance;
                    }
                    return content;
                }
                set
                {
                    if (value == null) throw new ArgumentNullException(nameof(value));

                    var original = Interlocked.CompareExchange(ref content, value, empty);

                    // Dispose if exchange failed.
                    if (original != empty)
                    {
                        value.Dispose();
                    }
                }
            }
        }

        Struct disposable;

        /// <summary>
        /// Gets a value indicating whether this is empty.
        /// </summary>
        public bool IsEmpty =>
            disposable.IsEmpty;

        /// <summary>
        /// Gets a value indicating whether this is disposed.
        /// </summary>
        public bool IsDisposed =>
            disposable.IsDisposed;

        /// <summary>
        /// Tries to dispose this and the content.
        /// Returns <c>true</c> if disposed.
        /// </summary>
        public bool TryDispose()
        {
            return disposable.TryDispose();
        }

        /// <summary>
        /// Disposes the content.
        /// </summary>
        public void Dispose()
        {
            disposable.Dispose();
        }

        /// <summary>
        /// <para>
        /// Getter: Gets the content.
        /// </para>
        /// <para>
        /// Setter: Sets the content.
        /// If this has content or is disposed, disposes the value instead.
        /// </para>
        /// </summary>
        /// <exception cref="ArgumentNullException" />
        public IDisposable Content
        {
            get { return disposable.Content; }
            set { disposable.Content = value; }
        }

        /// <summary>
        /// Constructs an empty disposable to own up to one disposable.
        /// </summary>
        public SingleDisposable()
        {
        }

        /// <summary>
        /// Constructs a disposable to own the specified disposable.
        /// </summary>
        public SingleDisposable(IDisposable disposable)
        {
            this.disposable.Content = disposable;
        }
    }
}
