using BenchmarkDotNet.Attributes;

namespace ConAppTest.MyBenchmarks;

[MemoryDiagnoser(false)]
public class StringSeparateByAiBenchmark : IBenchmark<StringSeparateByAiBenchmark>
{
    private readonly string _testCase = "The_quick_brown_fox-jumps_over_the_lazy-dog. This_sentence_is_often_used_as_a_test_for_typographers_or_designers,_as_it_contains_every_letter_of_the_English_alphabet_and_demonstrates_various_fonts_and_layouts.";

    [Benchmark]
    public void OldWay()
        => _ = this._testCase.Separate();
}