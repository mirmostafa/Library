#nullable disable

namespace ConAppTest;

public sealed class Just<T> : Maybe<T>
{
    public Just(T value)
        => this.Value = value;

    public override T Value { get; }
}

public abstract class Maybe<T>
{
    public abstract T Value { get; }

    public static implicit operator T(Maybe<T> self)
        => self.Value;
}

public sealed class Nothing<T> : Maybe<T>
{
    public override T Value => default;
}

public static class Extensions
{
    public static Maybe<TToType> Do<TFromType, TToType>(this Maybe<TFromType> selft, Func<TFromType, TToType> bind)
    {
        switch (selft)
        {
            case Just<TFromType> st when !EqualityComparer<TFromType>.Default.Equals(st.Value, default):
                try
                {
                    return bind(st).Maybe();
                }
                catch
                {
                    return new Nothing<TToType>();
                }
            default:
                return new Nothing<TToType>();
        }
    }

    public static Maybe<T> Maybe<T>(this T value)
        => new Just<T>(value);
}