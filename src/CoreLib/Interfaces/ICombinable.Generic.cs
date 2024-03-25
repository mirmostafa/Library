namespace Library.Interfaces;

public interface ICombinable<in TOther, out TResult>
{
    TResult Combine(TOther obj);
}

public interface ICombinable<TTSelf>
{
    TTSelf Combine(TTSelf obj);
}