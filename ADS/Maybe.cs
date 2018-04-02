using System;
using System.Collections.Generic;
using System.Text;

namespace ADS
{
    public interface Maybe<T> {
        bool IsJust();
        bool IsNothing();
        T Extract();
    }

    public static class _Maybe<T>
    {
        public static Maybe<T> From(T value)
        {
            //return null == value ? new Nothing<T>() : new Just<T>(value);
            return null;
        }

        public static Maybe<T> Of(T value)
        {
            return new Just<T>(value);
        }
    }
}
