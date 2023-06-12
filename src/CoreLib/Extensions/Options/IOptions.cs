namespace Library.Extensions.Options;

public interface IOptions
{ }

public interface IConfigurable<TSelf, TOptions>
    where TSelf : IConfigurable<TSelf, TOptions>
    where TOptions : IOptions
{
    TSelf Configure(Action<TOptions> configure);
}