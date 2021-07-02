using System;

namespace Lotos.Abstractions.Exceptions
{
    public class ConnectLotosException : LotosException
    {
        public ConnectLotosException(string message) : base($"Can't connect to Database because {message}", 1)
        {
            
        }

        public ConnectLotosException() : base("Can't connect to Database")
        {

        }
    }
}
