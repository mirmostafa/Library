namespace Library.CodeGeneration.v2.Models;

public interface INamespace
{
    string Name { get; }
    IEnumerable<IType> Types { get; }
    IEnumerable<INamespace> Namespaces { get; }
}

public interface IAttribute
{
    string Name { get; }
}

public interface IHasAttribute
{
    IEnumerable<IAttribute> Attributes { get; }
}

public interface IType : IHasAttribute
{
    string Name { get; }
    IEnumerable<IMember> Members { get; }
    IEnumerable<IType>? BaseTypes { get; }
    bool IsPartial { get; }
    bool IsSealed { get; }
    bool IsAbstract { get; }
}

public interface IClass : IType
{
}

public interface IStruct : IType
{
}

public interface IMember : IHasAttribute
{
    string Name { get; }
    AcceessModifier AcceessModifier { get; }
    bool IsStatic { get; }
    bool IsAbstract { get; }
}

public interface IMethod : IMember
{
    IType ReturnType { get; }
    IEnumerable<(IType Type, string Name)> Parameters { get; }
    IEnumerable<string>? Body { get; }
    bool IsConstructor { get; }
}

public interface IProperty : IMember
{
    IType Type { get; }
    bool HasGetter { get; }
    bool HasSetter { get; }
    string? BackingFieldName { get; }
}

public interface IField : IMember
{
    IType Type { get; }
    bool IsReadonly { get; }
    bool IsConst { get; }
}

public enum AcceessModifier
{
    Public,
    Private,
    Protected,
    Internal,
    ProtectedInternal,
    ProtectedPrivate,
    PrivateProtected,
    None
}

public interface ICodeGenerator
{
    string Generate(INamespace @namespace);

    Code Generate(INamespace @namespace, string name, Language language, bool isPartial)
        => Code.ToCode(name, language, this.Generate(@namespace), isPartial);
}