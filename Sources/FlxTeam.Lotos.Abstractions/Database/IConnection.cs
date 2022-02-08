using System;
using System.Threading.Tasks;

namespace FlxTeam.Lotos.Abstractions.Database;

public interface IConnection : IDisposable
{
    IBox<TEntity> GetBox<TEntity>() where TEntity : Entity<TEntity>;
    Task<IBox<TEntity>> GetBoxAsync<TEntity>() where TEntity : Entity<TEntity>;
}