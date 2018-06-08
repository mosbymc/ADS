using System;

namespace ADS.Combined
{
    public class Either<T> : IMonad<T>
    {
        private T Value;
        private Exception Ex;
        private bool HasRight;

        public Either()
        {
            HasRight = false;
            Value = default(T);
        }

        public Either(T value)
        {
            if (typeof(T).GetType().IsInstanceOfType(typeof(Exception)))
            {
                HasRight = false;
                Ex = value as Exception;
            }
            else
            {
                HasRight = true;
                Value = value;
            }
        }

        public Either(Exception ex)
        {
            Ex = ex;
            HasRight = false;
        }

        public IMonad<TResult> Map<TResult>(Func<T, TResult> fn)
        {
            if (HasRight)
            {
                try
                {
                    return new Either<TResult>(fn(Value));
                }
                catch (Exception ex)
                {
                    return new Either<TResult>(ex);
                }
            }
            return new Either<TResult>(Ex);
        }

        public Either<TResult> Chain<TResult>(Func<T, Either<TResult>> fn)
        {
            if (HasRight)
            {
                try
                {
                    return fn(Value);
                }
                catch (Exception ex)
                {
                    return new Either<TResult>(ex);
                }
            }
            return new Either<TResult>(Ex);
        }

        IMonad<TResult> IMonad<T>.Chain<TResult>(Func<T, IMonad<TResult>> fn)
        {
            return Chain(a => (Either<TResult>)fn(a));
        }

        public IMonad<Func<A, C>> Contramap<A, B, C>(Func<A, B> fn)
        {
            return new Either<Func<A, C>>(arg => (Value as Func<B, C>)(fn(arg)));
        }

        public IMonad<Func<A, D>> Dimap<A, B, C, D>(Func<A, B> f, Func<C, D> g)
        {
            return new Either<Func<A, D>>(arg => g((Value as Func<B, C>)(f(arg))));
        }

        public IMonad<Either<TResult>> Traverse<TResult>(Func<T, IMonad<TResult>> fn)
        {
            return fn(Value).Map(x => new Either<TResult>(x));
        }

        IMonad<IMonad<TResult>> IMonad<T>.Traverse<TResult>(Func<T, IMonad<TResult>> fn)
        {
            return Traverse(fn) as IMonad<IMonad<TResult>>;
        }

        public Either<TResult> Apply<TResult>(Either<T> x)
        {
            if (HasRight)
            {
                try
                {
                    return x.Map(Value as Func<T, TResult>) as Either<TResult>;
                }
                catch (Exception ex)
                {
                    return new Either<TResult>(ex);
                }
            }
            return new Either<TResult>(Ex);
        }

        IMonad<TResult> IMonad<T>.Apply<TResult>(IMonad<T> ma)
        {
            return Apply<TResult>(x: (Either<T>)ma);
        }

        public TResult Fold<TResult>(Func<T, TResult> fn)
        {
            return fn(Value);
        }

        public bool Equals(IMonad<T> ma)
        {
            return ma.GetType().IsInstanceOfType(this) && ma.Extract().Equals(Value);
        }

        public bool IsRight()
        {
            return HasRight;
        }

        public bool IsLeft()
        {
            return !HasRight;
        }

        public T Extract()
        {
            return Value;
        }

        public override string ToString()
        {
            return HasRight ? "Right(" + Value.ToString() + ")" : "Left";
        }
    }
}
