namespace Library.Helpers;

public static class BooleanHelper
{
    /// <summary>Determines whether any element of a sequence exists or satisfies a Boolean condition.</summary>
    /// <param name="bools">The bools.</param>
    /// <returns>
    ///   <br />
    /// </returns>
    public static bool Any(params bool[] bools)
        => bools.Any();
}