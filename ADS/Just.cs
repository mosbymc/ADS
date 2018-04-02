using System;
using System.Collections.Generic;
using System.Text;

namespace ADS
{
    public class Just<T> : Maybe<T>
    {
        private T Value;

        public Just(T value)
        {
            this.Value = value;
        }

        public Just<TResult> Map<TResult>(Func<T, TResult> fn)
        {
            return new Just<TResult>(fn(Value));
        }

        public Just<TResult> Chain<TResult>(Func<T, Just<TResult>> fn)
        {
            return fn(Value);
        }

        public Just<Func<A, C>> Contramap<A, B, C>(Func<A, B> fn)
        {
            return new Just<Func<A, C>>(arg => (Value as Func<B, C>)(fn(arg)));
        }

        /*
        public Just<T> Join()
        {
            //this.value.
            return Value.GetType().IsInstanceOfType(typeof(Just)) ? Value : this;
        }
        */

        public bool Equals(Maybe<T> j)
        {
            return j.IsJust() && Value.Equals(j.Extract());
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
            return Value;
        }
    }
}
