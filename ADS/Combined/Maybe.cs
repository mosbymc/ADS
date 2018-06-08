using System;
using System.Collections.Generic;
using System.Text;

namespace ADS.Combined
{
    public class Maybe<T> : IMonad<T>
    {
        private T Value;
        private bool HasValue;

        public Maybe()
        {
            HasValue = false;
            Value = default(T);
        }

        public Maybe(T value)
        {
            HasValue = null == value;
            Value = value;
        }

        public IMonad<TResult> Map<TResult>(Func<T, TResult> fn)
        {
            return new Maybe<TResult>(fn(Value));
        }

        public Maybe<TResult> Chain<TResult>(Func<T, Maybe<TResult>> fn)
        {
            if (HasValue) return fn(Value);
            return new Maybe<TResult>();
        }

        IMonad<TResult> IMonad<T>.Chain<TResult>(Func<T, IMonad<TResult>> fn)
        {
            return Chain(a => (Maybe<TResult>)fn(a));
        }

        public IMonad<Func<A, C>> Contramap<A, B, C>(Func<A, B> fn)
        {
            return new Maybe<Func<A, C>>(arg => (Value as Func<B, C>)(fn(arg)));
        }

        public IMonad<Func<A, D>> Dimap<A, B, C, D>(Func<A, B> f, Func<C, D> g)
        {
            return new Maybe<Func<A, D>>(arg => g((Value as Func<B, C>)(f(arg))));
        }

        public IMonad<Maybe<TResult>> Traverse<TResult>(Func<T, IMonad<TResult>> fn)
        {
            return fn(Value).Map(x => new Maybe<TResult>(x));
        }

        IMonad<IMonad<TResult>> IMonad<T>.Traverse<TResult>(Func<T, IMonad<TResult>> fn)
        {
            return Traverse(fn) as IMonad<IMonad<TResult>>;
            //var t = fn(Value).Map(x => new Identity<TResult>(x));
            //return t as IMonad<IMonad<TResult>>;
        }

        public Maybe<TResult> Apply<TResult>(Maybe<T> x)
        {
            return x.Map(Value as Func<T, TResult>) as Maybe<TResult>;
        }

        IMonad<TResult> IMonad<T>.Apply<TResult>(IMonad<T> ma)
        {
            return Apply<TResult>(x: (Maybe<T>)ma);
        }

        public TResult Fold<TResult>(Func<T, TResult> fn)
        {
            return fn(Value);
        }

        public bool Equals(IMonad<T> ma)
        {
            return ma.GetType().IsInstanceOfType(this) && ma.Extract().Equals(Value);
        }

        public bool IsJust()
        {
            return HasValue;
        }

        public bool IsNothing()
        {
            return !HasValue;
        }

        public T Extract()
        {
            return Value;
        }

        public override string ToString()
        {
            return HasValue ? "Just(" + Value.ToString() + ")" : "Nothing";
        }
    }
}
