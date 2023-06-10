namespace Library.Extensions.Options;

public interface IOptions
{ }

public interface IConfigurable<TConfigurable, TOptions>
    where TConfigurable : IConfigurable<TConfigurable, TOptions>
    where TOptions : IOptions
{
    TConfigurable Configure(Action<TOptions> configure);
}