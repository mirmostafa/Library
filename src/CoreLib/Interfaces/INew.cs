namespace Library.Interfaces;

public interface INew<out TSelf>
{
    static abstract TSelf New();
}

public interface INew<out TSelf, in TArg>
{
    static abstract TSelf New(TArg arg);
}

public interface INew<out TSelf, in TArg1, in TArg2>
{
    static abstract TSelf New(TArg1 arg1, TArg2 arg2);
}

public interface INew<out TSelf, in TArg1, in TArg2, in TArg3>
{
    static abstract TSelf New(TArg1 arg1, TArg2 arg2, TArg3 arg3);
}