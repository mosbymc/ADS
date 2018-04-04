using System;
using System.Collections.Generic;
using System.Text;

namespace ADS
{
    public static class Just
    {
        public static Just<T> Of<T>(T value)
        {
            return new Just<T>(value);
        }

        public static Just<T> Join<T>(this Just<Just<T>> source)
        {
            return source.Extract();
        }
    }

    public class Just<T> : Maybe<T>
    {
        private T Value;

        public Just(T value)
        {
            Value = value;
        }

        public IMonad<TResult> Map<TResult>(Func<T, TResult> fn)
        {
            var res = fn(Value);
            if (null == res) return new Nothing<TResult>();
            return new Just<TResult>(res);
        }

        public Maybe<TResult> Chain<TResult>(Func<T, Maybe<TResult>> fn)
        {
            return fn(Value);
        }

        IMonad<TResult> IMonad<T>.Chain<TResult>(Func<T, IMonad<TResult>> fn)
        {
            return Chain(a => (Maybe<TResult>)fn(a));
        }

        public IMonad<Func<A, C>> Contramap<A, B, C>(Func<A, B> fn)
        {
            return new Just<Func<A, C>>(arg => (Value as Func<B, C>)(fn(arg)));
        }

        public IMonad<Func<A, D>> Dimap<A, B, C, D>(Func<A, B> f, Func<C, D> g)
        {
            return new Just<Func<A, D>>(arg => g((Value as Func<B, C>)(f(arg))));
        }

        public IMonad<Just<TResult>> Traverse<TResult>(Func<T, IMonad<TResult>> fn)
        {
            return fn(Value).Map(x => new Just<TResult>(x));
        }

        IMonad<IMonad<TResult>> IMonad<T>.Traverse<TResult>(Func<T, IMonad<TResult>> fn)
        {
            return Traverse(fn) as IMonad<IMonad<TResult>>;
            //var t = fn(Value).Map(x => new Identity<TResult>(x));
            //return t as IMonad<IMonad<TResult>>;
        }

        public Just<TResult> Apply<TResult>(Just<T> x)
        {
            return x.Map(Value as Func<T, TResult>) as Just<TResult>;
        }

        IMonad<TResult> IMonad<T>.Apply<TResult>(IMonad<T> ma)
        {
            return Apply<TResult>(x: (Just<T>)ma);
        }

        public bool Equals(IMonad<T> ma)
        {
            return ma.GetType().IsInstanceOfType(this) && ma.Extract().Equals(Value);
        }

        public bool IsJust()
        {
            return true;
        }

        public bool IsNothing()
        {
            return false;
        }

        public T Extract()
        {
            return Value;
        }
    }
}
