using Library.Collections;
using Library.Types;

namespace UnitTests;

[Trait("Category", nameof(Library.Coding))]
[Trait("Category", nameof(Library.Collections))]
[Trait("Category", nameof(FluentList<int>))]
public sealed class FluentListTest
{
    [Fact]
    public void AddAndCount()
    {
        var list = FluentList<int>.New().Add(1000);
        Assert.Equal(1, list.Count);
        Assert.Equal(1000, list[0]);

        _ = list.Add(2000).Add(3000);
        Assert.Equal(3, list.Count);
        Assert.Equal(1000, list[0]);
        Assert.Equal(2000, list[1]);
        Assert.Equal(3000, list[2]);
    }

    [Fact]
    public void AddTestUsingObjectInitializer()
    {
        var list = FluentList<int>.New()
            .AddRange([1000, 2000, 3000]);
        Assert.Equal(1000, list[0]);
        Assert.Equal(2000, list[1]);
        Assert.Equal(3000, list[2]);
    }

    [Fact]
    public void AsList()
    {
        var list = new List<int>().AsFluent().Add(1000).Add(2000).AsList();
        Assert.True(list is not null);
        Assert.Equal(2, list.Count);
        Assert.Contains(2000, list);
    }

    [Fact]
    public void Build()
    {
        var list = new List<int>().AsFluent().Add(1000).Add(2000).Build();
        Assert.True(list is not null);
        Assert.Equal(2, list.Count);
        Assert.Contains(2000, list);
    }

    [Fact]
    public void Clear()
    {
        var list = FluentList<int>.New().Add(1000).Add(2000).Add(3000).Clear();
        Assert.Equal(0, list.Count);
    }

    [Fact]
    public void Contains()
    {
        var list = FluentList<int>.New().Add(1000).Add(2000).Add(3000);
        var has2000 = list.Contains(1000).Result;
        Assert.True(has2000);
        var has4000 = list.Contains(4000).Result;
        Assert.False(has4000);
    }

    [Fact]
    public void Create()
    {
        var empty = FluentList<int>.New();
        Assert.Empty(empty);

        var list = FluentList<int>.New([1000, 2000, 3000]);
        Assert.NotEmpty(list);
    }

    [Fact]
    public void ExplicitCast()
    {
        var list = new List<int>
        {
            1000, 2000
        };
        var fluent = list.AsFluent().Add(3000).Add(4000);
        list = fluent;
        Assert.True(list is not null);
        Assert.Equal(4, list.Count);
        Assert.Contains(2000, list);
    }

    [Fact]
    public void Indexer()
    {
        var list = FluentList<int>.New().Add(1000).Add(2000).Add(3000);
        Assert.Equal(3000, list[2]);
        list[2] = 4000;
        Assert.Equal(4000, list[2]);
    }

    [Fact]
    public void IndexOf()
    {
        var list = FluentList<int>.New().Add(1000).Add(2000).Add(3000);
        Assert.Equal(1, list.IndexOf(2000).Result);
    }

    [Fact]
    public void Insert()
    {
        var list = FluentList<int>.New().Add(1000).Add(2000).Add(3000).Insert(2, 4000);
        Assert.Equal(2, list.IndexOf(4000).Result);
        Assert.Equal(3, list.IndexOf(3000).Result);
    }

    [Fact]
    public void Iteration()
        => FluentList<int>.New().Add(1000).Add(2000).Add(3000).ForEach(Actions.Empty<int>());

    [Fact]
    public void Remove()
    {
        var list = FluentList<int>.New().Add(1000).Add(2000).Add(3000).Remove(2000);
        Assert.False(list.Contains(2000).Result);
    }

    [Fact]
    public void RemoveAt()
    {
        var list = FluentList<int>.New().Add(1000).Add(2000).Add(3000).RemoveAt(1);
        Assert.Equal(3000, list[1]);
    }
}