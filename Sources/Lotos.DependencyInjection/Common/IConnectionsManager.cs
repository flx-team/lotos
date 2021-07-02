using System;
using Lotos.Abstractions.Database;

namespace Lotos.DependencyInjection.Common
{
    public interface IConnectionsManager : IDisposable
    {
        int Add(IConnection connection);
        IConnection Get(int pin);
        void Remove(int pin);
    }
}