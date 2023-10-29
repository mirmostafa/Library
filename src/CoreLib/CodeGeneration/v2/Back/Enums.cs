namespace Library.CodeGeneration.v2.Back;

[Flags]
public enum AccessModifier
{
    None = 0,
    Private = 1,
    Protected = 2,
    Internal = 4,
    Public = 8,
    InternalProtected = Protected | Internal,
    ReadOnly = 32
}

[Flags]
public enum InheritanceModifier
{
    None = 0,
    Virtual = 1,
    Abstract = 2,
    Override = 4,
    New = 8,
    Sealed = 16,
    Static = 32,
    Partial = 64,
    Const = 128
}