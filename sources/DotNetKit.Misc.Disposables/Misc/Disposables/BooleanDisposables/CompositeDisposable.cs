using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetKit.Misc.Disposables
{
    /// <summary>
    /// Represents a mutable collection of disposables.
    /// Thread-safe.
    /// </summary>
    public sealed class CompositeDisposable
        : IBooleanDisposable
    {
        internal const int ShrinkThreshold = 32;

        static readonly IDisposable removedItem = new EmptyDisposable();

        readonly object gate = new object();

        /// <summary>
        /// Lock the gate to access.
        /// <c>null</c> if disposed.
        /// </summary>
        List<IDisposable> disposables;

        int count;

        /// <summary>
        /// Gets the number of items.
        /// </summary>
        public int Count => count;

        /// <summary>
        /// Gets a value indicating whether this is disposed.
        /// </summary>
        public bool IsDisposed => disposables == null;

        /// <summary>
        /// Gets the capacity.
        /// </summary>
        internal int Capacity
        {
            get
            {
                lock (gate)
                {
                    return disposables?.Capacity ?? 0;
                }
            }
        }

        /// <summary>
        /// Adds a disposable to be disposed with this.
        /// If this is already disposed, disposes it instead.
        /// </summary>
        /// <exception cref="ArgumentNullException" />
        public void Add(IDisposable disposable)
        {
            if (disposable == null) throw new ArgumentNullException(nameof(disposable));

            if (!IsDisposed)
            {
                lock (gate)
                {
                    if (!IsDisposed)
                    {
                        disposables.Add(disposable);
                        count++;
                        return;
                    }
                }
            }

            disposable.Dispose();
        }

        /// <summary>
        /// Shrinks the underlying list if sparse.
        /// Invoked in a lock.
        /// </summary>
        void Shrink()
        {
            if (count >= ShrinkThreshold && disposables.Capacity >= count * 2)
            {
                var newList = new List<IDisposable>(count);
                foreach (var d in disposables)
                {
                    if (d == removedItem) continue;
                    newList.Add(d);
                }

                disposables = newList;
            }
        }

        bool IsEmptyOrDisposed => disposables == null || count == 0;

        /// <summary>
        /// Tries to remove the first item equal to the specified disposable.
        /// Returns <c>true</c> if removed.
        /// </summary>
        public bool Remove(IDisposable disposable)
        {
            if (disposable == null || IsEmptyOrDisposed) return false;

            lock (gate)
            {
                if (IsEmptyOrDisposed) return false;

                var index = disposables.IndexOf(disposable);
                if (index < 0) return false;

                // Replace the item with a marker.
                // Change the length of list only when it's sparse for performance.
                disposables[index] = removedItem;
                count--;
                Shrink();
                return true;
            }
        }

        /// <summary>
        /// Tries to dispose all items.
        /// </summary>
        public bool TryDispose()
        {
            if (IsDisposed) return false;

            var list = default(List<IDisposable>);

            lock (gate)
            {
                if (IsDisposed) return false;

                list = disposables;
                disposables = null;
                count = 0;
            }

            foreach (var disposable in list)
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

        CompositeDisposable(List<IDisposable> disposables)
        {
            this.disposables = disposables;
        }

        /// <summary>
        /// Constructs a composite disposable with the specified capacity.
        /// </summary>
        public CompositeDisposable(int capacity)
            : this(new List<IDisposable>(capacity))
        {
        }

        /// <summary>
        /// Constructs a composite disposable,
        /// adding disposables of the specified sequence.
        /// </summary>
        public CompositeDisposable(IEnumerable<IDisposable> disposables)
            : this(new List<IDisposable>(disposables))
        {
        }

        /// <summary>
        /// Constructs an empty composite disposable.
        /// </summary>
        public CompositeDisposable()
            : this(new List<IDisposable>())
        {
        }
    }
}
