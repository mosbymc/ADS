using System;

namespace ADS
{
    public static class Identity
    {
        public static Identity<T> From<T>(T value)
        {
            return new Identity<T>(value);
        }

        public static Identity<T> Of<T>(T value)
        {
            return new Identity<T>(value);
        }

        public static Identity<T> Join<T>(this Identity<Identity<T>> source)
        {
            return source.Extract();
        }
    }

    public class Identity<T> : IMonad<T>
    {
        private T Value;

        public Identity(T value)
        {
            Value = value;
        }

        public Identity<T> Of(T value)
        {
            return new Identity<T>(value);
        }
        
        public IMonad<TResult> Map<TResult>(Func<T, TResult> fn)
        {
            return new Identity<TResult>(fn(Value));
        }

        public Identity<TResult> Chain<TResult>(Func<T, Identity<TResult>> fn)
        {
            var i = new Identity<Identity<int>>(new Identity<int>(5));
            var ii = new Identity<int>(5).Map(x => new Identity<int>(x));
            return fn(Value);
        }

        IMonad<TResult> IMonad<T>.Chain<TResult>(Func<T, IMonad<TResult>> fn)
        {
            return Chain(a => (Identity<TResult>)fn(a));
        }

        public IMonad<Func<A, C>> Contramap<A, B, C>(Func<A, B> fn)
        {
            return new Identity<Func<A, C>>(arg => (Value as Func<B, C>)(fn(arg)));
        }

        public IMonad<Func<A, D>> Dimap<A, B, C, D>(Func<A, B> f, Func<C, D> g)
        {
            return new Identity<Func<A, D>>(arg => g((Value as Func<B, C>)(f(arg))));
        }

        public IMonad<Identity<TResult>> Traverse<TResult>(Func<T, IMonad<TResult>> fn)
        {
            return fn(Value).Map(x => new Identity<TResult>(x));
        }

        IMonad<IMonad<TResult>> IMonad<T>.Traverse<TResult>(Func<T, IMonad<TResult>> fn)
        {
            return Traverse(fn) as IMonad<IMonad<TResult>>;
            //var t = fn(Value).Map(x => new Identity<TResult>(x));
            //return t as IMonad<IMonad<TResult>>;
        }

        public Identity<TResult> Apply<TResult>(Identity<T> x)
        {
            return x.Map(Value as Func<T, TResult>) as Identity<TResult>;
        }

        IMonad<TResult> IMonad<T>.Apply<TResult>(IMonad<T> ma)
        {
            return Apply<TResult>(x: (Identity<T>)ma);
        }
        
        public TResult Fold<TResult>(Func<T, TResult> fn)
        {
            return fn(Value);
        }

        public bool Equals(IMonad<T> ma)
        {
            return ma.GetType().IsInstanceOfType(this) && ma.Extract().Equals(Value);
        }

        public T Extract()
        {
            /*
            Identity<int> i = new Identity<int>(10);
            Identity<Func<int, int>> f = new Identity<Func<int, int>>(x => x * x);

            i.Map(x => x - 5)
                .Chain(x => new Identity<int>(x * x))
                .Apply(f);

            Func<int, int> gn = x => x + 5;

            f.Contramap<int, int, int>(gn);
            */

            return Value;
        }

        public override string ToString()
        {
            return "Identity(" + Value.ToString() + ")";
        }
    }
}
