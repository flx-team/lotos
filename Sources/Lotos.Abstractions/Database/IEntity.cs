using System;

namespace Lotos.Abstractions.Database
{
    public interface IEntity<T> : IEntityIdentifier, IEntityProvider<T> where T : IEntity<T>
    {
        
    }
}
