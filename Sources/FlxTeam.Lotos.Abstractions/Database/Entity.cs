using System;
using FlxTeam.Lotos.Abstractions.Attributes;

namespace FlxTeam.Lotos.Abstractions.Database;

public class Entity<T> : IEntity where T : Entity<T>
{
    public Guid Id { get; set; }

    [Ignore]
    public IBox<T>? Basket { get; set; }
}