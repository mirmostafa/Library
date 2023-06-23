#nullable disable

using System.Collections;

using Library.DesignPatterns.Markers;

namespace Library.Coding;

/// <summary>
/// </summary>
[Fluent]
[Obsolete("Subject to remove", true)]
public readonly struct Args<T> : IReadOnlyList<T>
{
    private readonly List<T> _arguments = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="Args{T}"/> struct.
    /// </summary>
    /// <param name="items">The items.</param>
    public Args(params T[] items)
        : this(items.AsEnumerable()) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Args{T}"/> struct.
    /// </summary>
    /// <param name="items">The items.</param>
    public Args(IEnumerable<T> items)
        => this._arguments.AddRange(items);

    /// <inheritdoc/>
    public int Count => ((IReadOnlyCollection<T>)this._arguments).Count;

    /// <inheritdoc/>
    public T this[int index] => this._arguments[index];

    public static implicit operator Args<T>(T arg1)
        => new(arg1);

    public static implicit operator Args<T>((T arg1, T arg2) args)
        => new(args.arg1, args.arg2);

    public static implicit operator Args<T>((T arg1, T arg2, T arg3) args)
        => new(args.arg1, args.arg2, args.arg3);

    public static implicit operator Args<T>((T arg1, T arg2, T arg3, T arg4) args)
        => new(args.arg1, args.arg2, args.arg3, args.arg4);

    public static implicit operator Args<T>((T arg1, T arg2, T arg3, T arg4, T arg5) args)
        => new(args.arg1, args.arg2, args.arg3, args.arg4, args.arg5);

    public static implicit operator Args<T>((T arg1, T arg2, T arg3, T arg4, T arg5, T arg6) args)
        => new(ObjectHelper.GetProperties(args).Select(x => x.Value).Cast<T>());

    public static implicit operator Args<T>((T arg1, T arg2, T arg3, T arg4, T arg5, T arg6, T arg7) args)
        => new(ObjectHelper.GetProperties(args).Select(x => x.Value).Cast<T>());

    public static implicit operator Args<T>((T arg1, T arg2, T arg3, T arg4, T arg5, T arg6, T arg7, T arg8) args)
        => new(ObjectHelper.GetProperties(args).Select(x => x.Value).Cast<T>());

    public static implicit operator Args<T>((T arg1, T arg2, T arg3, T arg4, T arg5, T arg6, T arg7, T arg8, T arg9) args)
        => new(ObjectHelper.GetProperties(args).Select(x => x.Value).Cast<T>());

    public static implicit operator Args<T>((T arg1, T arg2, T arg3, T arg4, T arg5, T arg6, T arg7, T arg8, T arg9, T arg10) args)
        => new(ObjectHelper.GetProperties(args).Select(x => x.Value).Cast<T>());

    /// <inheritdoc/>
    public IEnumerator<T> GetEnumerator()
        => this._arguments.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => this._arguments.GetEnumerator();
}