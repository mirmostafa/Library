namespace Library.Interfaces;

public interface ISupportEmpty<TClass>
{
    static abstract TClass Empty();
    static abstract TClass NewEmpty();
}
