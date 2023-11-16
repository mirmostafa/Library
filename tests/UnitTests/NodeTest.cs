using System.Diagnostics;

using Library.Collections;

using Xunit.Abstractions;

namespace UnitTests;


[Trait("Category", nameof(Library.Collections))]
public sealed class NodeTest
{
    private readonly string[] _array = new string[10];
    private readonly ITestOutputHelper _output;

    public NodeTest(ITestOutputHelper output)
    {
        _array[0] = "00";
        _array[1] = "0001";
        _array[2] = "0002";
        _array[3] = "0003";
        _array[4] = "000201";
        _array[5] = "000101";
        _array[6] = "00010101";
        _array[7] = "000202";
        _array[8] = "000301";
        _array[9] = "00020201";
        this._output = output;
    }

    [Fact]
    public void FlattenTest()
    {
        var node = Node<string>.Create(() => _array[0], s => _array.Where(x => x.StartsWith(s) && x.Length == s.Length + 2));
        _output.WriteLine(node);
    }
}