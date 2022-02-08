using System;

namespace FlxTeam.Lotos.Abstractions.Exceptions;

public class ConnectLotosException : LotosException
{
    public ConnectLotosException(string message, Exception? innerException = null)
        : base($"Can't connect to the database because {message}", innerException) { }

    public ConnectLotosException(Exception? innerException = null)
        : base("Can't connect to database", innerException) { }
}