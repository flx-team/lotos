using System;

namespace FlxTeam.Lotos.Abstractions.Database;

public interface IEntity
{
	Guid Id { get; set; }
}