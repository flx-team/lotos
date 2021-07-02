using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lotos.Abstractions.Database;
using Lotos.DependencyInjection.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Lotos.Examples.AspNetCore.Controllers
{
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

        private readonly IBasket<UserEntity> _usersBasket;      

        public WeatherForecastController(IConnectionsManager manager)
        {
            var connection1 = manager.Get(1);
            var connection0 = manager.Get(0);

            _usersBasket = connection0.GetBasket<UserEntity>().GetAwaiter().GetResult();
        }

        [HttpGet]
        public async Task Get()
        {
            var user = new UserEntity()
            {
                Name = "Roman",
            };

            await _usersBasket.Keep(user);

            user.Name = "Roman2";

            await _usersBasket.Sync(user);

            var user2 = await _usersBasket.Pick(e => e.Name == "Roman2");
        }
    }
}
