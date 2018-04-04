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

    public class Constant<T>
    {
        private T Value;

        public Constant(T value)
        {
            Value = value;
        }

        public Constant<TResult> Map<TResult>(Func<T, TResult> fn)
        {
            new Constant<T>(Value);
            return new Constant<TResult>(fn(Value));
        }

        public Constant<T> Chain<TResult>(Func<T, Constant<TResult>> fn)
        {
            return this;
        }

        public Constant<T> Contramap<A, B>(Func<A, B> fn)
        {
            return this;
        }

        public Constant<T> Dimap<A, B, C, D>(Func<A, B> f, Func<C, D> g)
        {
            return this;
        }

        public TResult Fold<TResult>(Func<T, TResult> fn)
        {
            return fn(Value);
        }

        public T Extract()
        {
            return Value;
        }

        public override string ToString()
        {
            return "Constant(" + Value.ToString() + ")";
        }
    }
}
