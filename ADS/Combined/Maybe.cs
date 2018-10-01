using System;

namespace ADS.Combined
{
    public class Maybe<T> : IMonad<T>
    {
        private readonly T _value;
        private readonly bool _hasValue;

        public Maybe()
        {
            _hasValue = false;
            _value = default(T);
        }

        public Maybe(T value)
        {
            _hasValue = null == value;
            _value = value;
        }

        public IMonad<TResult> Map<TResult>(Func<T, TResult> fn)
        {
            return new Maybe<TResult>(fn(_value));
        }

        public Maybe<TResult> Chain<TResult>(Func<T, Maybe<TResult>> fn)
        {
            if (_hasValue) return fn(_value);
            return new Maybe<TResult>();
        }

        IMonad<TResult> IMonad<T>.Chain<TResult>(Func<T, IMonad<TResult>> fn)
        {
            return Chain(a => (Maybe<TResult>)fn(a));
        }

        public IMonad<Func<A, C>> Contramap<A, B, C>(Func<A, B> fn)
        {
            return new Maybe<Func<A, C>>(arg => (_value as Func<B, C>)(fn(arg)));
        }

        public IMonad<Func<A, D>> Dimap<A, B, C, D>(Func<A, B> f, Func<C, D> g)
        {
            return new Maybe<Func<A, D>>(arg => g((_value as Func<B, C>)(f(arg))));
        }

        public IMonad<Maybe<TResult>> Traverse<TResult>(Func<T, IMonad<TResult>> fn)
        {
            return fn(_value).Map(x => new Maybe<TResult>(x));
        }

        IMonad<IMonad<TResult>> IMonad<T>.Traverse<TResult>(Func<T, IMonad<TResult>> fn)
        {
            return Traverse(fn) as IMonad<IMonad<TResult>>;
            //var t = fn(Value).Map(x => new Identity<TResult>(x));
            //return t as IMonad<IMonad<TResult>>;
        }

        public Maybe<TResult> Apply<TResult>(Maybe<T> x)
        {
            return x.Map(_value as Func<T, TResult>) as Maybe<TResult>;
        }

        IMonad<TResult> IMonad<T>.Apply<TResult>(IMonad<T> ma)
        {
            return Apply<TResult>(x: (Maybe<T>)ma);
        }

        public TResult Fold<TResult>(Func<T, TResult> fn)
        {
            return fn(_value);
        }

        public bool Equals(IMonad<T> ma)
        {
            return ma.GetType().IsInstanceOfType(this) && ma.Extract().Equals(_value);
        }

        public bool IsJust()
        {
            return _hasValue;
        }

        public bool IsNothing()
        {
            return !_hasValue;
        }

        public T Extract()
        {
            return _value;
        }

        public override string ToString()
        {
            return _hasValue ? "Just(" + _value.ToString() + ")" : "Nothing";
        }
    }
}
