using System;
using FlxTeam.Lotos.Abstractions.Database;

namespace FlxTeam.Lotos.DependencyInjection.Common;

public interface IConnectionsManager : IDisposable
{
    int Add(IConnection connection);
    IConnection Get(int pin);
    void Remove(int pin);
}