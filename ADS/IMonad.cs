using System;
using System.Collections.Generic;
using System.Text;

namespace ADS
{
    public interface IMonad<T>
    {
        IMonad<TResult> Map<TResult>(Func<T, TResult> fn);
        IMonad<TResult> Chain<TResult>(Func<T, IMonad<TResult>> fn);
        IMonad<Func<A, C>> Contramap<A, B, C>(Func<A, B> fn);
        IMonad<Func<A, D>> Dimap<A, B, C, D>(Func<A, B> f, Func<C, D> g);
        IMonad<IMonad<TResult>> Traverse<TResult>(Func<T, IMonad<TResult>> fn);
        IMonad<TResult> Apply<TResult>(IMonad<T> ma);
        T Extract();
        bool Equals(IMonad<T> ma);
        string ToString();
    }
}
