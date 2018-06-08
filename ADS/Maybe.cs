using System;

namespace ADS
{
    public interface Maybe<T> : IMonad<T>
    {
        bool IsJust();
        bool IsNothing();
    }

    public static class Maybe
    {
        public static Maybe<T> From<T>(T value)
        {
            if (null == value) return new Nothing<T>();
            return new Just<T>(value);
        }

        public static Maybe<T> Of<T>(T value)
        {
            return new Just<T>(value);
        }

        public static Just<T> Just<T>(T value)
        {
            return new Just<T>(value);
        }

        public static Nothing<T> Nothing<T>(T value)
        {
            return new Nothing<T>();
        }
    }
}
