using System;
using System.Collections.Generic;
using System.Text;

namespace ADS
{
    public class Constant<T>
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
