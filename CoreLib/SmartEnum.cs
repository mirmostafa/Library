namespace Library;

public abstract class SmartEnum<TSmartEnum, TId> : IEquatable<TSmartEnum>
    where TSmartEnum : SmartEnum<TSmartEnum, TId>
    where TId : notnull
{
    public SmartEnum(TId id, string? friendlyName)
        => (this.Id, this.FriendlyName) = (id, friendlyName);

    public TId Id { get; }
    public string? FriendlyName { get; }

    public bool Equals(TSmartEnum? other) =>
        other is not null
        && (this.Id is null
                ? other.Id is null
                : this.Id.Equals(other.Id));

    public static bool operator ==(SmartEnum<TSmartEnum, TId>? enum1, SmartEnum<TSmartEnum, TId>? enum2) =>
        enum1?.Equals(enum2) ?? enum2 is null;

    public static bool operator !=(SmartEnum<TSmartEnum, TId>? enum1, SmartEnum<TSmartEnum, TId>? enum2) =>
        !(enum1 == enum2);

    public override bool Equals(object? obj) =>
        obj is SmartEnum<TSmartEnum, TId> other && this.Equals(other);

    public override int GetHashCode() =>
        this.Id?.GetHashCode() ?? default;

    public static TSmartEnum? ById(TId id) =>
        typeof(TSmartEnum).GetFields(System.Reflection.BindingFlags.Static)
                          .Where(x => x.FieldType == typeof(TSmartEnum))
                          .Select(x => x.GetValue(null).As<TSmartEnum>())
                          .Where(x => x is not null && (x.Id?.Equals(id) ?? id is null))
                          .SingleOrDefault();
}

public abstract class SmartEnum<TSmartEnum> : SmartEnum<TSmartEnum, int>
    where TSmartEnum : SmartEnum<TSmartEnum>
{
    protected SmartEnum(int id, string? friendlyName)
        : base(id, friendlyName)
    {
    }
}
