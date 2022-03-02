using System.Runtime.Versioning;

namespace Library.Interfaces;

[RequiresPreviewFeatures]
public interface IConvertible<TThis, TOther>
{
    TOther ConvertTo();
    static abstract TThis ConvertFrom([DisallowNull] TOther other);
}
