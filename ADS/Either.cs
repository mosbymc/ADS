using System;
using System.Collections.Generic;
using System.Text;

namespace ADS
{
    public interface Either<T> : IMonad<T>
    {
        bool IsLeft();
        bool IsRight();
    }

    public static class Either
    {

    }
}
