using System;

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
        private readonly T _value;

        public Just(T value)
        {
            _value = value;
        }

        public IMonad<TResult> Map<TResult>(Func<T, TResult> fn)
        {
            var res = fn(_value);
            if (null == res) return new Nothing<TResult>();
            return new Just<TResult>(res);
        }

        public Maybe<TResult> Chain<TResult>(Func<T, Maybe<TResult>> fn)
        {
            return fn(_value);
        }

        IMonad<TResult> IMonad<T>.Chain<TResult>(Func<T, IMonad<TResult>> fn)
        {
            return Chain(a => (Maybe<TResult>)fn(a));
        }

        public IMonad<Func<A, C>> Contramap<A, B, C>(Func<A, B> fn)
        {
            return new Just<Func<A, C>>(arg => (_value as Func<B, C>)(fn(arg)));
        }

        public IMonad<Func<A, D>> Dimap<A, B, C, D>(Func<A, B> f, Func<C, D> g)
        {
            return new Just<Func<A, D>>(arg => g((_value as Func<B, C>)(f(arg))));
        }

        public IMonad<Just<TResult>> Traverse<TResult>(Func<T, IMonad<TResult>> fn)
        {
            return fn(_value).Map(x => new Just<TResult>(x));
        }

        IMonad<IMonad<TResult>> IMonad<T>.Traverse<TResult>(Func<T, IMonad<TResult>> fn)
        {
            return Traverse(fn) as IMonad<IMonad<TResult>>;
            //var t = fn(Value).Map(x => new Identity<TResult>(x));
            //return t as IMonad<IMonad<TResult>>;
        }

        public Just<TResult> Apply<TResult>(Just<T> x)
        {
            return x.Map(_value as Func<T, TResult>) as Just<TResult>;
        }

        IMonad<TResult> IMonad<T>.Apply<TResult>(IMonad<T> ma)
        {
            return Apply<TResult>(x: (Just<T>)ma);
        }

        public TResult Fold<TResult>(Func<T, T, TResult> fn, T seed)
        {
            return fn(seed, _value);
        }

        public bool Equals(IMonad<T> ma)
        {
            return ma.GetType().IsInstanceOfType(this) && ma.Extract().Equals(_value);
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
            return _value;
        }
    }
}
