﻿using Library.Helpers;
using Library.MultistepProgress;

internal partial class Program
{
    private static async Task Main()
    {
        var add5 = (int x) => x + 5;
        var init = () => 5;

        var result = await TaskRunner<int>.New()
            .StartWith(init.ToAsync())
            .Then(add5.ToAsync())
            .Then(add5.ToAsync())
            .RunAsync();
        Console.WriteLine(result.Value);
    }
}