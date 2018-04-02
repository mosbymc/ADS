using System;
using System.Collections.Generic;
using System.Text;
//using System.Linq;

namespace ADS
{
    public static class Identity
    {
        public static Identity<T> Of<T>(T value)
        {
            return new Identity<T>(value);
        }
    }

    public class Identity<T>
    {
        private T Value;

        public Identity(T value)
        {
            //List<List<int>> p = new List<List<int>>();
            //p.SelectMany(x => x);
            Value = value;
        }

        /*
        public Identity<T> Of(T value)
        {
            //Func<int, Identity<int>> pp = Identity.Of<int>;
            return new Identity<T>(value);
        }*/
        
        public Identity<TResult> Map<TResult>(Func<T, TResult> fn)
        {
            //Identity<Identity<int>> pp = new Identity<Identity<int>>(new Identity<int>(1));
            //var tt = pp.Map(x => x.Map(y => y * y));
            return new Identity<TResult>(fn(Value));
        }

        public Identity<TResult> Chain<TResult>(Func<T, Identity<TResult>> fn)
        {
            //Identity<Identity<int>> i = new Identity<Identity<int>>(new Identity<int>(5));
            //var rr = i.Join3<Identity<Identity<int>>>();
            return fn(Value);
        }

        public Identity<Func<A, C>> Contramap<A, B, C>(Func<A, B> fn)
        {
            return new Identity<Func<A, C>>(arg => (Value as Func<B, C>)(fn(arg)));
        }

        /*
        public Identity<Func<A, D>> Dimap<A, B, C, D>(Func<A, B> f, Func<C, D> g)
        {
            return new Identity<Func<A, D>>(arg => g((Value as Func<B, C>)(f(arg))));
        }*/

        /*
        public Constant<TResult> Traverse<TResult>(Func<T, Constant<TResult>> fn)
        {
            //var p = new Identity<Identity<int>>(new Identity<int>(1));
            //var tt = p.Join2<int>();
            //return fn(Value).Map(Identity.Of);
            return null;
        }*/

        public Identity<TResult> Apply<TResult>(Identity<Func<T, TResult>> x)
        {
            return Map(x.Extract() as Func<T, TResult>) as Identity<TResult>;
        }

        /*
        public Identity<TResult> Apply<TResult>(Identity<T> x)
        {
            var p = new Identity<int>(1);
            var yyy = p.Map(d => new Identity<int>(d));

            return x.Map(Value as Func<T, TResult>) as Identity<TResult>;
        }*/

        /*
        public Identity<T> Join3(this Identity<Identity<T>> source)
        {
            //var p = source.Value;
            return source.Value;
        }
        */

        /*
        public Identity<U> Join2<U>()
        {
            //Func<Identity<U>, Identity<U>> identity = tt => tt;
            //var ppp = Chain<Identity<U>>(identity);

            return Value as Identity<U>;
            //var x = Value.GetType().IsInstanceOfType(this) ? Value as Identity<U> : new Identity<U>(Value);
            //return x;
        }*/

        /*
        public Identity<T> Join<T>() where T : Identity<T>
        {
            return Value.GetType().IsInstanceOfType(this) ? Value as Identity<T> : new Identity<T>(Value);
        }
        */

            /*
        public TResult Fold<TResult>(Func<T, TResult> fn)
        {
            return fn(Value);
        }
        */

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
