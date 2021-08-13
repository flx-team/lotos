using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading.Tasks;

using Lotos.Abstractions.Attributes;
using Lotos.Abstractions.Database;
using Lotos.Abstractions.Exceptions;
using Lotos.Mongo.Conventions;

using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace Lotos.Mongo.Database
{
    public sealed class MongoDriver : IDriver
    {
        static MongoDriver()
        {
            ConventionRegistry.Register("LotosMongoConventions", new ConventionPack { new EntityClassMapConvention() }, t =>
            {
                return t.GetInterfaces().Any(x =>
                    x.IsGenericType &&
                    x.GetGenericTypeDefinition() == typeof(IEntity<>));
            });
        }

        public string ConnectionString { get; }

        public string DatabaseName { get; }
        public int MillisecondsTimeout { get; }

        private readonly ConnectLotosException _connException;

        private readonly Assembly _assembly;

        public MongoDriver(string connectionString, string databaseName, int millisecondsTimeout = 10000, Assembly? assembly = null)
        {
            ConnectionString = connectionString;
            DatabaseName = databaseName;
            MillisecondsTimeout = millisecondsTimeout;

            _assembly = assembly ?? Assembly.GetCallingAssembly();

            _connException = new ConnectLotosException(
                $"can't connect to mongo database server with connection string '{ConnectionString}' and with timeout {MillisecondsTimeout} milliseconds.");
        }

        public Task<IConnection> Run()
        {
            try
            {
                var client = new MongoClient(ConnectionString);

                var db = client.GetDatabase(DatabaseName);

                if (!IsMongoLive(db, MillisecondsTimeout))
                {
                    throw _connException;
                }

                return Task.FromResult((IConnection)new MongoConnection(db));
            }
            catch (MongoException)
            {
                throw _connException;
            }
        }

        private bool IsMongoLive(IMongoDatabase mongoDatabase, int millisecondsTimeout)
        {
            return mongoDatabase.RunCommandAsync((Command<BsonDocument>)"{ping:1}")
                .Wait(millisecondsTimeout);
        }
    }
}
