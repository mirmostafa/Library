﻿using System.Numerics;

namespace ConAppTest;

internal partial class Program
{
    private static void Main(string[] args)
    {
    }

    private static IEnumerable<TResult> SimdActor<T1, T2, TResult>(
        IEnumerable<T1> left, IEnumerable<T2> right, Func<Vector<T1>, Vector<T2>, Vector<TResult>> actor, long count)
        where T1 : struct
        where T2 : struct
        where TResult : struct
    {
        var leftArray = left.ToArray();
        var rightArray = right.ToArray();
        var result = new TResult[count];
        var offset = Vector<TResult>.Count;
        for (var i = 0; i < count - offset; i += offset)
        {
            var v1 = new Vector<T1>(leftArray, i);
            var v2 = new Vector<T2>(rightArray, i);
            actor(v1, v2).CopyTo(result, i);
        }
        return result;
    }
}