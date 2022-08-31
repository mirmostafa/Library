namespace Library.Helpers;

public static class BooleanHelper
{
    public static bool Any(params bool[] bools)
        => bools.Any();
    public static bool IsTrue(this bool? condition)
        => condition is true;
}