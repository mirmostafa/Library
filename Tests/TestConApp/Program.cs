﻿using BenchmarkDotNet.Running;
using LibraryTest;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace TestConApp;

internal partial class Program
{
    private static ILogger _logger = null!;

    public static async Task Main()
        => BenchmarkRunner.Run<StringHelperTest>();
}