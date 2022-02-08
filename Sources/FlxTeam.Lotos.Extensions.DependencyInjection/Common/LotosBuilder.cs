using FlxTeam.Lotos.Abstractions.Database;
using Microsoft.Extensions.DependencyInjection;

namespace FlxTeam.Lotos.DependencyInjection.Common;

public sealed class LotosBuilder
{
    private readonly IServiceCollection _services;
    private readonly IConnectionsManager _manager;

    public LotosBuilder(IServiceCollection services)
    {
        _services = services;
        _manager = new ConnectionsManager();
    }

    public void Init()
    {
        InitConnectionsManager();
    }

    private void InitConnectionsManager()
    {
        _services.AddSingleton(_manager);
    }

    public LotosBuilder AttachDatabase<T>(T driver) where T : IDriver
    {
        var connection = driver.CreateConnection();

        _manager.Add(connection);

        return this;
    }

    public LotosBuilder AttachDatabase<T>(T driver, out int pin) where T : IDriver
    {
        var connection = driver.CreateConnection();

        pin = _manager.Add(connection);

        return this;
    }

    public LotosBuilder SingleDatabase<T>(T driver) where T : IDriver
    {
        var connection = driver.CreateConnection();

        _services.AddSingleton(connection);

        return this;
    }
}