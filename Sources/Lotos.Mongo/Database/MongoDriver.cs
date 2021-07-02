using System;
using System.Threading.Tasks;
using Lotos.Abstractions.Database;
using Lotos.Abstractions.Exceptions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace Lotos.Mongo.Database
{
    public sealed class MongoDriver : IDriver
    {
        public string ConnectionString { get; }

        public string DatabaseName { get; }
        public int MillisecondsTimeout { get; }

        private readonly ConnectLotosException _connException;

        public MongoDriver(string connectionString, string databaseName, int millisecondsTimeout = 10000)
        {
            ConnectionString = connectionString;
            DatabaseName = databaseName;
            MillisecondsTimeout = millisecondsTimeout;

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
