using System;

namespace ADS
{
    public class Left<T>
    {
        private T Value;

        public Left(T value)
        {
            Value = value;
        }

        public IMonad<TResult> Map<TResult>(Func<T, TResult> fn)
        {
            return new Right<TResult>(fn(Value));
        }

        public Either<TResult> Chain<TResult>(Func<T, Either<TResult>> fn)
        {
            return fn(Value);
        }

        public IMonad<Func<A, C>> Contramap<A, B, C>(Func<A, B> fn)
        {
            return new Right<Func<A, C>>(arg => (Value as Func<B, C>)(fn(arg)));
        }

        public IMonad<Func<A, D>> Dimap<A, B, C, D>(Func<A, B> f, Func<C, D> g)
        {
            return new Right<Func<A, D>>(arg => g((Value as Func<B, C>)(f(arg))));
        }

        public IMonad<Right<TResult>> Traverse<TResult>(Func<T, IMonad<TResult>> fn)
        {
            return fn(Value).Map(x => new Right<TResult>(x));
        }

        public Right<TResult> Apply<TResult>(Right<T> x)
        {
            return x.Map(Value as Func<T, TResult>) as Right<TResult>;
        }

        public bool Equals(IMonad<T> ma)
        {
            return ma.GetType().IsInstanceOfType(this) && ma.Extract().Equals(Value);
        }

        public bool IsRight()
        {
            return true;
        }

        public bool IsLeft()
        {
            return false;
        }

        public T Extract()
        {
            return Value;
        }
    }
}
