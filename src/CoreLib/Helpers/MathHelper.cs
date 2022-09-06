namespace Library.Helpers;

public static class MathHelper
{
    public static long Factorial(in long num) => num <= 1 ? num : num * Factorial(num - 1);
}