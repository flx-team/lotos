using System;
using System.Threading.Tasks;
using Lotos.Abstractions.Attributes;
using Lotos.Abstractions.Database;
using Lotos.Mongo.Database;
using InOut = System.Console;

namespace Lotos.Examples.Console
{
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

            using (var connection = await driver.Run())
            {
                var usersBasket = await connection.GetBasket<UserEntity>();

                var user = new UserEntity()
                {
                    Name = "Name1",
                    IgnoredName = "Ignored",
                };

                await usersBasket.Keep(user);

                InOut.ReadLine();

                user.Name = "Name2";

                await user.Sync();

                // user = await user.Pick();

                InOut.ReadLine();

                await user.Remove();
            }
        }
    }
}
