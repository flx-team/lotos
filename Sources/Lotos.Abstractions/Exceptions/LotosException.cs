using System;

namespace Lotos.Abstractions.Exceptions
{
    public class LotosException : Exception
    {
        public LotosException() : base("none")
        {

        }

        public LotosException(string message, int code = 0) : base(message)
        {
            
        }
    }
}
