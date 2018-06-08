using System;

namespace ADS
{
    public static class Constant
    {
        public static Constant<T> Of<T>(T value)
        {
            return new Constant<T>(value);
        }
    }

    public class Constant<T> : IMonad<T>
    {
        private T Value;

        public Constant(T value)
        {
            Value = value;
        }

        public Constant<T> Map<TResult>(Func<T, TResult> fn)
        {
            return this;
        }

        public Constant<T> Chain<TResult>(Func<T, Constant<TResult>> fn)
        {
            return this;
        }

        IMonad<TResult> IMonad<T>.Chain<TResult>(Func<T, IMonad<TResult>> fn)
        {
            return fn(Value);
        }

        public Constant<T> Contramap<A, B>(Func<A, B> fn)
        {
            return this;
        }

        public Constant<T> Dimap<A, B, C, D>(Func<A, B> f, Func<C, D> g)
        {
            return this;
        }

        public IMonad<Constant<TResult>> Traverse<TResult>(Func<T, IMonad<TResult>> fn)
        {
            return fn(Value).Map(x => new Constant<TResult>(x));
        }

        IMonad<IMonad<TResult>> IMonad<T>.Traverse<TResult>(Func<T, IMonad<TResult>> fn)
        {
            return Traverse(fn) as IMonad<IMonad<TResult>>;
        }

        public Constant<T> Apply<TResult>(Constant<T> x)
        {
            return this;
        }

        IMonad<TResult> IMonad<T>.Apply<TResult>(IMonad<T> ma)
        {
            return ma.Map(Value as Func<T, TResult>) as Constant<TResult>;
        }

        public TResult Fold<TResult>(Func<T, TResult> fn)
        {
            return fn(Value);
        }

        public T Extract()
        {
            return Value;
        }

        public bool Equals(IMonad<T> ma)
        {
            return ma.GetType().IsInstanceOfType(this) && ma.Extract().Equals(Value);
        }

        public override string ToString()
        {
            return "Constant(" + Value.ToString() + ")";
        }
    }
}
