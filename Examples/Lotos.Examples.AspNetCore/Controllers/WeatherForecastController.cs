using System.Threading.Tasks;
using FlxTeam.Lotos.Abstractions.Database;
using FlxTeam.Lotos.DependencyInjection.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Lotos.Examples.AspNetCore.Controllers;

class UserEntity : Entity<UserEntity>
{
    public string Name { get; set; }
}

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    private readonly IBox<UserEntity> _usersBasket;      

    public WeatherForecastController(IConnectionsManager manager, ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
        var connection1 = manager.Get(1);
        var connection0 = manager.Get(0);

        _usersBasket = connection0.GetBox<UserEntity>();
    }

    [HttpGet]
    public async Task Get()
    {
        var user = new UserEntity()
        {
            Name = "Roman",
        };

        await _usersBasket.PutAsync(user);

        user.Name = "Roman2";

        await _usersBasket.SyncAsync(user);

        var user2 = await _usersBasket.PickAsync(e => e.Name == "Roman2");
    }
}