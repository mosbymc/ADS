using System;

namespace ADS
{
    public static class Nothing
    {
        public static Nothing<T> Of<T>(T value)
        {
            return new Nothing<T>(value);
        }

        public static Nothing<T> Join<T>(this Nothing<Nothing<T>> source)
        {
            return source.Extract();
        }
    }

    public class Nothing<T> : Maybe<T>
    {
        private readonly T _value;

        public Nothing() {}

        public Nothing(T value)
        {
            _value = value;
        }

        public Maybe<TResult> Map<TResult>(Func<T, TResult> fn)
        {
            return new Nothing<TResult>();
        }

        IMonad<TResult> IMonad<T>.Map<TResult>(Func<T, TResult> fn)
        {
            return Map(fn);
        }

        public Maybe<TResult> Chain<TResult>(Func<T, Maybe<TResult>> fn)
        {
            return new Nothing<TResult>();
        }

        IMonad<TResult> IMonad<T>.Chain<TResult>(Func<T, IMonad<TResult>> fn)
        {
            return Chain(a => (Maybe<TResult>)fn(a));
        }

        public IMonad<Func<A, C>> Contramap<A, B, C>(Func<A, B> fn)
        {
            return new Nothing<Func<A, C>>();
        }

        public IMonad<Func<A, D>> Dimap<A, B, C, D>(Func<A, B> f, Func<C, D> g)
        {
            return new Nothing<Func<A, D>>();
        }

        public IMonad<Nothing<TResult>> Traverse<TResult>(Func<T, IMonad<TResult>> fn)
        {
            return fn(_value).Map(x => new Nothing<TResult>());
        }

        IMonad<IMonad<TResult>> IMonad<T>.Traverse<TResult>(Func<T, IMonad<TResult>> fn)
        {
            return Traverse(fn) as IMonad<IMonad<TResult>>;
            //var t = fn(Value).Map(x => new Identity<TResult>(x));
            //return t as IMonad<IMonad<TResult>>;
        }

        public Nothing<TResult> Apply<TResult>(Nothing<T> x)
        {
            return new Nothing<TResult>();
        }

        IMonad<TResult> IMonad<T>.Apply<TResult>(IMonad<T> ma)
        {
            return Apply<TResult>(x: (Nothing<T>)ma);
        }

        public Nothing<TResult> Fold<TResult>(Func<T, T, TResult> fn, T seed)
        {
            return new Nothing<TResult>(default(TResult));
        }

        public bool Equals(IMonad<T> ma)
        {
            return ma.GetType().IsInstanceOfType(this) && ma.Extract().Equals(_value);
        }

        public bool IsJust()
        {
            return false;
        }

        public bool IsNothing()
        {
            return true;
        }

        public T Extract()
        {
            return _value;
        }
    }
}
