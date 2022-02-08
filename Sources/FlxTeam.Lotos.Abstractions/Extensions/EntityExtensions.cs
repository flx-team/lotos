using System;
using System.Threading.Tasks;
using FlxTeam.Lotos.Abstractions.Database;

namespace FlxTeam.Lotos.Abstractions.Extensions;

public static class EntityExtensions
{
	public static void Sync<TEntity>(this TEntity entity) where TEntity : Entity<TEntity>
	{
		entity.Basket!.Sync(entity);
	}
	
	public static Task SyncAsync<TEntity>(this TEntity entity) where TEntity : Entity<TEntity>
	{
		return entity.Basket!.SyncAsync(entity);
	}
	
	public static void Remove<TEntity>(this TEntity entity) where TEntity : Entity<TEntity>
	{
		entity.Basket!.Remove((Guid) entity.Id!);
	}
	
	public static Task RemoveAsync<TEntity>(this TEntity entity) where TEntity : Entity<TEntity>
	{
		return entity.Basket!.RemoveAsync((Guid) entity.Id!);
	}
	
	public static TEntity Pick<TEntity>(this TEntity entity) where TEntity : Entity<TEntity>
	{
		return entity.Basket!.Pick((Guid) entity.Id!)!;
	}
	
	public static async Task<TEntity> PickAsync<TEntity>(this TEntity entity) where TEntity : Entity<TEntity>
	{
		return (await entity.Basket!.PickAsync((Guid) entity.Id!))!;
	}
}