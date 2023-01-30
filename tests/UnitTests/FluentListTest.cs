using Library.Collections;

namespace UnitTests;


public class FluentListTest
{
    private FluentList<int> _List;
    private static readonly Func<int, int> _selfIntFunc = x => x;
    private static readonly Action<int> _emptyIntAction = x => { };

    public FluentListTest()
    {
        this._List = FluentList<int>.Create(Enumerable.Range(0, 10));
    }

    [Fact]
    public void IndexerTest() => Assert.Equal(5, this._List[5]);

    [Fact]
    public void CountTest() => Assert.Equal(10, this._List.Count);

    [Fact]
    public void CreateTest()
    {
        _ = FluentList<int>.Create();
        _ = FluentList<int>.Create(this._List);
    }

    [Fact]
    public void IndexOfTest()
    {
        var (_, result) = this._List.IndexOf(5);
        Assert.Equal(5, result);
    }

    [Fact]
    public void InsertTest() => this._List.Insert(5, 5);

    [Fact]
    public void RemoveTest() => this._List.Remove(5);

    [Fact]
    public void RemoveAtTest() => this._List.RemoveAt(5);

    [Fact]
    public void AddTest() => this._List.Add(5);

    [Fact]
    public void ClearTest() => this._List.Clear();

    [Fact]
    public void ContainsTest() => Assert.True(this._List.Contains(4).Result);

    [Fact]
    public void IterationTest() => this._List.ForEach(_emptyIntAction);
}
