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

public interface INew<out TSelf, in TArg1, in TArg2, in TArg3, in TArg4>
{
    static abstract TSelf New(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4);
}

public interface INew<out TSelf, in TArg1, in TArg2, in TArg3, in TArg4, in TArg5>
{
    static abstract TSelf New(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5);
}