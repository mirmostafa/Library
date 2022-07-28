namespace Library.Helpers;

public static class MathHelper
{
    public static long Factorial(in long num) 
        => num == 0 ? 1 : num * Factorial(num - 1);
}