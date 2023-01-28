namespace Library.Interfaces;

public interface IConvertible<out T>
{
    T Convert();
}

//public interface IConvertible<in TSelf, out TDestination>
//{
//    static abstract implicit operator TDestination(TSelf source);
//}

public interface IConvetert<in TSource, out TDestination>
{
    abstract static TDestination Convert(TSource source);
}