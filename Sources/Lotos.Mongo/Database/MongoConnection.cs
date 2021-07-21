using System;
using System.Threading.Tasks;
using Lotos.Abstractions.Database;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace Lotos.Mongo.Database
{
    internal sealed class MongoConnection : IConnection
    {
        private IMongoDatabase _database;

        public MongoConnection(IMongoDatabase database)
        {
            _database = database;
        }

        public void Dispose()
        {

        }

        public Task<IBasket<T>> GetBasket<T>() where T : IEntity<T>
        {
            var type = typeof(T);

            var collectionName = type.Name;

            var collection = _database.GetCollection<T>(collectionName);

            var basket = new MongoBasket<T>(collection);

            return Task.FromResult((IBasket<T>)basket);
        }
    }
}
