using Library.Collections;

namespace UnitTests;

[Trait("Category", nameof(Library.Coding))]
public sealed class FluentListTest
{
    private static readonly Action<int> _emptyIntAction = x => { };
    private static readonly Func<int, int> _selfIntFunc = x => x;
    private readonly FluentList<int> _list;

    public FluentListTest() 
        => this._list = FluentList<int>.Create(Enumerable.Range(0, 10));

    [Fact]
    public void AddTest()
        => this._list.Cast().To<IList<int>>().Add(5);

    [Fact]
    public void ClearTest()
        => this._list.Clear();

    [Fact]
    public void ContainsTest()
        => Assert.True(this._list.Contains(4).Result);

    [Fact]
    public void CountTest()
        => Assert.Equal(10, this._list.Count);

    [Fact]
    public void CreateTest()
    {
        _ = FluentList<int>.Create();
        _ = FluentList<int>.Create(this._list);
    }

    [Fact]
    public void IndexerTest()
        => Assert.Equal(5, this._list[5]);

    [Fact]
    public void IndexOfTest()
    {
        var (_, result) = this._list.IndexOf(5);
        Assert.Equal(5, result);
    }

    [Fact]
    public void InsertTest()
        => this._list.Insert(5, 5);

    [Fact]
    public void IterationTest()
        => this._list.ForEach(_emptyIntAction);

    [Fact]
    public void RemoveAtTest()
        => this._list.RemoveAt(5);

    [Fact]
    public void RemoveTest()
        => this._list.Remove(5);
}