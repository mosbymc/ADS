using System;
using System.Collections.Generic;
using System.Text;

namespace ADS
{
    public class Nothing<T> : Maybe<T>
    {
        private T value;

        public Nothing() {}

        public Nothing(T value)
        {

        }

        public bool IsJust()
        {
            return false;
        }

        public bool IsNothing()
        {
            return true;
        }

        public T Extract()
        {
            return this.value;
        }
    }
}
