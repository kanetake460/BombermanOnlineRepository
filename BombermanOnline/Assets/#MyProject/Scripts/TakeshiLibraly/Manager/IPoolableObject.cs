using System;

public interface IPoolableObject<T>
{
    T Get(Func<T, bool> isTakeble, Func<T> generator);
}