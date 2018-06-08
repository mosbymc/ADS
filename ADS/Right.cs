using System;

namespace ADS
{
    public static class Right
    {
        public static Right<T> Of<T>(T value)
        {
            return new Right<T>(value);
        }

        public static Right<T> Join<T>(this Right<Right<T>> source)
        {
            return source.Extract();
        }
    }

    public class Right<T> : Either<T>
    {
        private T Value;

        public Right(T value)
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

        IMonad<TResult> IMonad<T>.Chain<TResult>(Func<T, IMonad<TResult>> fn)
        {
            return Chain(a => (Either<TResult>)fn(a));
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

        IMonad<IMonad<TResult>> IMonad<T>.Traverse<TResult>(Func<T, IMonad<TResult>> fn)
        {
            return Traverse(fn) as IMonad<IMonad<TResult>>;
        }

        public Right<TResult> Apply<TResult>(Right<T> x)
        {
            return x.Map(Value as Func<T, TResult>) as Right<TResult>;
        }

        IMonad<TResult> IMonad<T>.Apply<TResult>(IMonad<T> ma)
        {
            return Apply<TResult>(x: (Right<T>)ma);
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
