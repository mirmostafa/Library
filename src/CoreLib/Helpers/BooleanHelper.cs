namespace Library.Helpers;

public static class BooleanHelper
{
    /// <summary>
    /// Checks if any of the boolean values in the given array is true.
    /// </summary>
    public static bool Any(params bool[] bools)
        => bools is not null and { Length: > 0 };
    /// <summary>
    /// Checks if a boolean condition is true.
    /// </summary>
    /// <param name="condition">The boolean condition to check.</param>
    /// <returns>True if the condition is true, false otherwise.</returns>
    public static bool IsTrue(this bool? condition)
        => condition is true;
}