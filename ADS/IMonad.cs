using System;
using System.Collections.Generic;
using System.Text;

namespace ADS
{
    public interface IMonad<T>
    {
        IMonad<TResult> Map<TResult>(Func<T, TResult> fn);
    }
}
