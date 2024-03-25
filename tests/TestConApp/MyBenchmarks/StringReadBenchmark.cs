using System;

using BenchmarkDotNet.Attributes;

namespace TestConApp.MyBenchmarks;

[MemoryDiagnoser(false)]
public class StringReadBenchmark
{
    private readonly string largeString = @"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed vel nisl auctor, bibendum sapien vel, aliquam nunc. Donec euismod, magna in luctus bibendum, velit sapien ultrices tellus, vel lacinia enim nisl nec velit. Nulla facilisi. Sed ac quam vel velit malesuada commodo. Sed euismod, metus quis aliquet bibendum, odio nulla tincidunt mauris, a malesuada nibh elit vitae magna. Praesent euismod, lectus eget suscipit bibendum, neque elit tincidunt nunc, non lacinia enim quam ut urna. Fusce euismod augue ut risus iaculis, id blandit libero ultricies. Donec quis mauris at ex blandit hendrerit. Sed euismod sapien vel justo malesuada, eget bibendum mi aliquam.

Sed vitae nibh et dolor pharetra faucibus. Donec in augue ac enim tincidunt malesuada. Nulla facilisi. Integer sit amet ante vel tellus ultrices varius. Sed eget dui euismod, cursus sapien sed, feugiat ipsum. Nullam auctor felis vitae purus dictum, non ultrices nulla vestibulum. Aliquam erat volutpat. Donec nec semper turpis.

Sed id enim at odio ullamcorper commodo nec ut nisi. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Sed auctor orci quis semper varius. Fusce at dolor eget est aliquet rhoncus ac sit amet libero. Donec sed risus sit amet orci eleifend luctus vitae vel velit. Praesent sed erat in velit maximus vehicula quis non urna.

Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia curae; Duis at ex ut quam interdum finibus a quis nibh. In hac habitasse platea dictumst. Fusce euismod felis sed purus efficitur, at pulvinar purus bibendum. Donec ut diam vel elit suscipit dapibus non eget eros.

Sed id lectus nec lorem imperdiet molestie nec vel justo. Nam eget leo id mi aliquet sagittis a in augue. Aliquam erat volutpat. Nullam eget tortor vitae massa eleifend dignissim non sed arcu. Suspendisse potenti. Sed ut est sit amet nulla tempor congue quis ac sapien.

Donec id dolor euismod, sagittis magna vitae, ornare nulla. Proin id justo vel massa mollis elementum at in urna. Duis maximus ex auctor tellus suscipit, in pharetra nisi bibendum. Curabitur blandit leo eget mauris consectetur varius.";

    [Benchmark]
    public void ReadLines()
    {
        var str = this.largeString;
        using var reader = new StringReader(str);
        string? line;
        while ((line = reader.ReadLine()) != null)
        {
            DoNothing(line);
        }
    }

    [Benchmark]
    public void ReadLinesBySpan()
    {
        var str = this.largeString;
        var span = str.AsSpan();
        var delimiter = Environment.NewLine.AsSpan();
        int index;
        ReadOnlySpan<char> line;
        while (!span.IsEmpty)
        {
            index = span.IndexOf(delimiter);
            line = index >= 0 ? span[..index] : span;
            span = index >= 0 ? span[(index + delimiter.Length)..] : ReadOnlySpan<char>.Empty;

            DoNothing(new(line));
        }
    }

    [Benchmark]
    public void ReadLinesBySpanInlineVars()
    {
        var str = this.largeString;
        var span = str.AsSpan();
        var delimiter = Environment.NewLine.AsSpan();

        while (!span.IsEmpty)
        {
            var index = span.IndexOf(delimiter);
            var line = index >= 0 ? span[..index] : span;
            span = index >= 0 ? span[(index + delimiter.Length)..] : ReadOnlySpan<char>.Empty;

            DoNothing(new(line));
        }
    }

    [Benchmark]
    public void ReadLinesBySplit()
    {
        foreach (var line in this.largeString.Split(Environment.NewLine))
        {
            DoNothing(line);
        }
    }

    [Benchmark]
    public void BingAI()
    {
        ReadOnlySpan<char> span = largeString.AsSpan();
        ReadOnlySpan<char> delimiter = Environment.NewLine.AsSpan();

        while (!span.IsEmpty)
        {
            int index = span.IndexOf(delimiter);
            ReadOnlySpan<char> line = index >= 0 ? span.Slice(0, index) : span;
            span = index >= 0 ? span.Slice(index + delimiter.Length) : ReadOnlySpan<char>.Empty;

            DoNothing(new(line));
        }
    }

    private static void DoNothing(string line)
    { }
}