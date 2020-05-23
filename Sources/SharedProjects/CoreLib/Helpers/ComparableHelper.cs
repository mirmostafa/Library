using System;

namespace Mohammad.Helpers
{
    /// <summary>
    ///     Contains extensions that make IComparable methods
    ///     a bit easier to remember.
    /// </summary>
    public static class ComparableHelper
    {
        /// <summary>
        ///     Determine if the left operand is greater than the right
        /// </summary>
        /// <typeparam name="T">the type being compared</typeparam>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>true if left &gt; right</returns>
        public static bool IsGreaterThan<T>(this T left, T right) where T : class, IComparable<T>
        {
            if (left == null)
                return false;

            return left.CompareTo(right) > 0;
        }

        /// <summary>
        ///     Determine if the left operand is less than the right
        /// </summary>
        /// <typeparam name="T">the type being compared</typeparam>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>true if left &lt; right </returns>
        public static bool IsLessThan<T>(this T left, T right) where T : class, IComparable<T>
        {
            if (left == null)
                return right != null;

            return left.CompareTo(right) < 0;
        }

        /// <summary>
        ///     Determines if the left operand is greater than or equal
        ///     than the right
        /// </summary>
        /// <typeparam name="T">the type being compared</typeparam>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>true if left &gt;= right</returns>
        public static bool IsGreaterThanOrEqual<T>(this T left, T right) where T : class, IComparable<T>
        {
            if (left == null)
                return right == null ? true : false;

            return left.CompareTo(right) >= 0;
        }

        /// <summary>
        ///     Determines if the left operand is less than or
        ///     equal to the right
        /// </summary>
        /// <typeparam name="T">the type being compared</typeparam>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>true if the left &lt;= right</returns>
        public static bool IsLessThanOrEqual<T>(this T left, T right) where T : class, IComparable<T>
        {
            if (left == null)
                return true;
            return left.CompareTo(right) <= 0;
        }
    }
}