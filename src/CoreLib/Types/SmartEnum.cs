using System.Diagnostics;

namespace Library.Types;

[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
public abstract class SmartEnum<TSmartEnum, TId>(TId id, string? friendlyName) : IEquatable<TSmartEnum>
    where TSmartEnum : SmartEnum<TSmartEnum, TId>
    where TId : notnull
{
    public string? FriendlyName { get; } = friendlyName;
    public TId Id { get; } = id;

    public static TSmartEnum? ById(TId id)
        => GetEnumItems().Where(x => x.Id?.Equals(id) ?? id is null).SingleOrDefault();

    public static IEnumerable<TSmartEnum> ByName(string? name)
        => GetEnumItems().Where(x => x.FriendlyName == name);

    public static bool operator !=(SmartEnum<TSmartEnum, TId>? enum1, SmartEnum<TSmartEnum, TId>? enum2)
        => !(enum1 == enum2);

    public static bool operator ==(SmartEnum<TSmartEnum, TId>? enum1, SmartEnum<TSmartEnum, TId>? enum2)
        => enum1?.Equals(enum2) ?? enum2 is null;

    public bool Equals(TSmartEnum? other)
        => other is not null
            && (this.Id is null
                ? other.Id is null
                : this.Id.Equals(other.Id));

    public override bool Equals(object? obj)
        => obj is SmartEnum<TSmartEnum, TId> other && this.Id.Equals(other.Id);

    public override int GetHashCode()
        => this.Id?.GetHashCode() ?? default;

    public override string ToString()
        => this.FriendlyName ?? base.ToString() ?? string.Empty;

    private static IEnumerable<TSmartEnum> GetEnumItems()
    {
        var type = typeof(TSmartEnum);
        //var staticFields = type.GetFields(System.Reflection.BindingFlags.Static);
        var staticFields = type.GetFields();
        var smartEnums = staticFields.Where(x => x.FieldType == typeof(TSmartEnum));
        var rawResult = smartEnums.Select(x => x.GetValue(null).Cast().As<TSmartEnum>());
        var result = rawResult.Compact();
        return result;
    }

    private string GetDebuggerDisplay()
        => this.ToString();
}

public abstract class SmartEnum<TSmartEnum> : SmartEnum<TSmartEnum, int>
    where TSmartEnum : SmartEnum<TSmartEnum>
{
    protected SmartEnum(int id, string? friendlyName)
        : base(id, friendlyName)
    {
    }
}