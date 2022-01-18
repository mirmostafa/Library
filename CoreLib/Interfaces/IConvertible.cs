using System.Diagnostics.CodeAnalysis;
using System.Runtime.Versioning;

namespace Library.Interfaces;

[RequiresPreviewFeatures]
public interface IConvertible<TThis, TOther>
{
    TOther Convert();
    static abstract TThis Convert([DisallowNull] TOther other);
}
