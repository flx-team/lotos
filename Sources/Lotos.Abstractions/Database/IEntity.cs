using System;

namespace Lotos.Abstractions.Database
{
    public interface IEntity<T> : IEntityIdentifier<object>, IEntityProvider<T> where T : IEntity<T>
    {
        
    }
}
