using static Library.Helpers.MathHelper.Outputs;

namespace Library.Helpers;

public static class MathHelper
{
    public static long Factorial(in long num) => num <= 1 ? num : num * Factorial(num - 1);

    public static class Equations
    {
        public static QuadraticEquation Quadratic(int a, int b, int c)
        {
            QuadraticEquation result = new QuadraticEquation();

            return result;
        }
    }

    public static class Outputs
    {
        public record struct QuadraticEquation(double x1, double x2, double delta);
    }
}