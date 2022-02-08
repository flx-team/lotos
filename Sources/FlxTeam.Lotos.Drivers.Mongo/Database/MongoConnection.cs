using System.Threading.Tasks;
using FlxTeam.Lotos.Abstractions.Database;
using MongoDB.Driver;

namespace FlxTeam.Lotos.Drivers.Mongo.Database;

internal sealed class MongoConnection : IConnection
{
    private readonly IMongoDatabase _database;

    public MongoConnection(IMongoDatabase database)
    {
        _database = database;
    }

    public IBox<TEntity> GetBox<TEntity>() where TEntity : Entity<TEntity>
    {
        var type = typeof(TEntity);

        var collectionName = type.Name;

        var collection = _database.GetCollection<TEntity>(collectionName);

        var basket = new MongoBox<TEntity>(this, collection);

        return basket;
    }
        
    public Task<IBox<TEntity>> GetBoxAsync<TEntity>() where TEntity : Entity<TEntity>
    {
        var type = typeof(TEntity);

        var collectionName = type.Name;

        var collection = _database.GetCollection<TEntity>(collectionName);

        var basket = new MongoBox<TEntity>(this, collection);

        return Task.FromResult((IBox<TEntity>) basket);
    }
        
    public void Dispose() { }
}