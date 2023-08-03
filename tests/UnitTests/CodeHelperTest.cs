﻿using Library.Coding;
using Library.Logging;
using Library.Results;

namespace UnitTests;

[Trait("Category", "Helpers")]
[Trait("Category", "Code Helpers")]
[Obsolete("Subject to remove", true)]
public sealed class CodeHelperTest
{
    [Fact]
    public async void LibStopwatchTest1()
    {
        var stopwatch = new LibStopwatch();
        using (stopwatch.Start())
        {
            await Task.Delay(1000);
        }
        var elapsed = stopwatch.Elapsed;
        Assert.True(elapsed.Ticks > 0, "Stopwatch is working like a charm!");
    }

    [Fact]
    public async Task LibStopwatchTest2Async()
    {
        var stopwatch = LibStopwatch.StartNew();
        await Task.Delay(1000);
        _ = stopwatch.Stop();
        var elapsed = stopwatch.Elapsed;
        Assert.True(elapsed.Ticks > 0);
    }

    [Fact]
    public void TryResult_02_Failure()
    {
        // Assign
        var divedBy = 0;
        void local_act()
        {
        }
        int local_fun()
            => 5 / divedBy;
        var an_act = () => { _ = 5 / divedBy; };
        var an_fun = () => 5 / divedBy;

        // Act
        var actual_an_act =CatchResult(an_act);
        var actual_an_fun = CatchResult(an_fun);
        var actual_local_act = CatchResult(ToAction(local_act));
        var actual_local_fun = CatchResult(ToFunc(local_fun));
        var exceptedExceptionType = typeof(DivideByZeroException);
        var expectedMessage = "Attempted to divide by zero.";

        // Assert
        Assert.NotNull(actual_an_act);
        Assert.NotNull(actual_an_fun);
        Assert.NotNull(actual_local_act);
        Assert.NotNull(actual_local_fun);
        var ex = Assert.IsAssignableFrom<DivideByZeroException>(actual_an_fun.Status);
        Assert.Equal(expectedMessage, ex.Message);
        ex = Assert.IsAssignableFrom<DivideByZeroException>(actual_local_fun.Status);
        Assert.Equal(expectedMessage, ex.Message);

    }

    [Fact]
    public void TryResult_01_Success()
    {
        // Assign
        var divedBy = 5;
        void local_act()
        {
        }
        int local_fun()
            => 5 / divedBy;
        var an_act = () => { _ = 5 / divedBy; };
        var an_fun = () => 5 / divedBy;

        // Act
        var actual_an_act = CatchResult(an_act);
        var actual_an_fun = CatchResult(an_fun);
        var actual_local_act = CatchResult(ToAction(local_act));
        var actual_local_fun = CatchResult(ToFunc(local_fun));

        // Assert
        Assert.NotNull(actual_an_act);
        Assert.NotNull(actual_an_fun);
        Assert.NotNull(actual_local_act);
        Assert.NotNull(actual_local_fun);
        Assert.Equal(1, actual_an_fun);
        Assert.Equal(1, actual_local_fun);

    }
}