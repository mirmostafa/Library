namespace Library.CodeGeneration.v2.Back;

[Flags]
public enum MemberPrefixes
{
    None = 0,
    Private = 1,
    Protected = 2,
    Internal = 4,
    Public = 8,

    Constant = 16,
    ReadOnly = 32,
    Ref = 64,

    Virtual = 128,
    Abstract = 256,
    Override = 512,
    New = 1024,
    Sealed = 2048,

    Partials = 4096,
}
