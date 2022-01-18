namespace Library.Interfaces;

public interface ISupportEmpty<TClass>
{
    static abstract TClass Empty { get; }
    static abstract TClass NewEmpty();
}
