using System.Collections;

using Library.Validations;

namespace Library.Data;

public abstract class ValueObject<TValueObject> : IEquatable<TValueObject>, IStructuralEquatable
    where TValueObject : ValueObject<TValueObject>
{
    public static bool operator !=(ValueObject<TValueObject> a, ValueObject<TValueObject> b) =>
        !(a == b);

    public static bool operator ==(ValueObject<TValueObject> a, ValueObject<TValueObject> b) =>
        a?.Equals(b) ?? false;

    public virtual bool Equals(TValueObject? other) =>
        other is { } o && o.GetHashCode() == this.GetHashCode();

    public virtual bool Equals(object? other, IEqualityComparer comparer) =>
        comparer?.Equals(this, other) ?? false;

    public override bool Equals(object? obj) =>
        ReferenceEquals(this, obj) || (obj is TValueObject o && this.Equals(o));

    public virtual int GetHashCode(IEqualityComparer comparer) =>
        comparer.ArgumentNotNull().GetHashCode(this);

    public override int GetHashCode() =>
        this.OnGetHashCode();

    protected abstract int OnGetHashCode();
}