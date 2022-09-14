using BenchmarkDotNet.Attributes;

namespace ConAppTest;

[MemoryDiagnoser(false)]
public class IsNullOrEmptyBenchmarks
{
    private readonly string _text = "Bug 325426: عدم نمایش مقدار اندیکاتور روی فرم با دسترسی محدود(مشتری جوش آزمای)";

    [Benchmark]
    public bool Comparison() => this._text == null || this._text.Length == 0; //! 🏆 Winner 

    [Benchmark]
    public bool PatternMatching() => this._text is not null and { Length: > 0 };

    [Benchmark]
    public bool PatternMatching2() => this._text?.Length is null or 0; // 🥈

    [Benchmark]
    public bool StringIsNullOrEmpty() => string.IsNullOrEmpty(this._text); // 🥉
}