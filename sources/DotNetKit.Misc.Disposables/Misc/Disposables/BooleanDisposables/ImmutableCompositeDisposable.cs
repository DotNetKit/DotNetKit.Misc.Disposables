using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetKit.Misc.Disposables
{
    /// <summary>
    /// Provides static methods.
    /// </summary>
    public static partial class ImmutableCompositeDisposable
    {
        /// <summary>
        /// Creates a boolean disposable.
        /// </summary>
        public static IBooleanDisposable Create()
        {
            return new BooleanDisposable();
        }

        /// <summary>
        /// Creates a boolean disposable to own the disposable.
        /// </summary>
        /// <exception cref="ArgumentNullException" />
        public static IBooleanDisposable Create(IBooleanDisposable disposable)
        {
            if (disposable is IBooleanDisposable booleanDisposable)
            {
                return booleanDisposable;
            }

            return new SingleDisposable(disposable);
        }

        /// <summary>
        /// Creates a boolean disposable to own the specified disposables.
        /// </summary>
        /// <exception cref="ArgumentException" />
        /// <exception cref="ArgumentNullException" />
        public static IBooleanDisposable Create(IDisposable disposable1, IDisposable disposable2, IDisposable disposable3, IDisposable disposable4, params IDisposable[] rest)
        {
            var array = new IDisposable[rest.Length + 4];

            array[0] = disposable1 ?? throw new ArgumentNullException(nameof(disposable1));
            array[1] = disposable2 ?? throw new ArgumentNullException(nameof(disposable2));
            array[2] = disposable3 ?? throw new ArgumentNullException(nameof(disposable3));
            array[3] = disposable4 ?? throw new ArgumentNullException(nameof(disposable4));

            if (rest == null) throw new ArgumentNullException(nameof(rest));
            for (var i = 0; i < rest.Length; i++)
            {
                if (rest[i] == null) throw new ArgumentException($"{nameof(rest)}[{i}] is null.");
                array[i + 4] = rest[i];
            }

            return new ArrayDisposable(array);
        }

        /// <summary>
        /// Creates a boolean disposable to own disposables of the specified sequence.
        /// </summary>
        /// <exception cref="ArgumentException" />
        /// <exception cref="ArgumentNullException" />
        public static IBooleanDisposable FromEnumerable(IEnumerable<IDisposable> disposables)
        {
            if (disposables == null) throw new ArgumentNullException(nameof(disposables));

            var array = disposables.ToArray();

            var index = Array.IndexOf(array, null);
            if (index >= 0) throw new ArgumentException($"{nameof(disposables)}[{index}] is null.");

            return new ArrayDisposable(array);
        }
    }
}
