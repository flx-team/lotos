using System;
using System.Threading.Tasks;
using FlxTeam.Lotos.Abstractions.Attributes;
using FlxTeam.Lotos.Abstractions.Database;
using FlxTeam.Lotos.Abstractions.Extensions;
using FlxTeam.Lotos.Drivers.Mongo.Database;
using InOut = System.Console;

namespace Lotos.Examples.Console;

class UserEntity : Entity<UserEntity>
{
    public string Name { get; set; }

    [Ignore]
    public string IgnoredName { get; set; }
}

class Program
{
    static void Main(string[] args)
    {
        AsyncMain(args).GetAwaiter().GetResult();
    }

    static async Task AsyncMain(string[] args)
    {
        var driver = new MongoDriver("mongodb://localhost:27017", "console_test_db");

        using var connection = await driver.CreateConnectionAsync();
        using var usersBasket = await connection.GetBoxAsync<UserEntity>();

        var user = new UserEntity()
        {
            Name = "Name1",
            IgnoredName = "Ignored",
        };

        await usersBasket.PutAsync(user);

        InOut.ReadLine();

        user.Name = "Name2";

        await user.SyncAsync();

        InOut.ReadLine();

        await user.RemoveAsync();
    }
}