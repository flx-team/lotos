using System;

namespace FlxTeam.Lotos.Abstractions.Exceptions;

public class LotosException : Exception
{
    public LotosException(string? message = null, Exception? innerException = null)
        : base(message, innerException) { }
}