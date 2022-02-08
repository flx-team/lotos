using System.Collections.Generic;
using FlxTeam.Lotos.Abstractions.Database;

namespace FlxTeam.Lotos.DependencyInjection.Common;

internal sealed class ConnectionsManager : IConnectionsManager
{
    private int _lastPin = 0;
    private readonly Dictionary<int, IConnection> _pins = new();

    public int Add(IConnection connection)
    {
        _pins.Add(_lastPin, connection);

        _lastPin++;

        return _lastPin;
    }

    public void Remove(int pin)
    {
        var connection = Get(pin);
        connection.Dispose();

        _pins.Remove(pin);
    }

    public IConnection Get(int pin)
    {
        return _pins[pin];
    }

    public void Dispose()
    {
        foreach (var element in _pins)
        {
            Remove(element.Key);
        }
    }
}