using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lotos.Abstractions.Database;
using Microsoft.Extensions.DependencyInjection;

namespace Lotos.DependencyInjection.Common
{
    public sealed class LotosBuilder
    {
        private readonly IServiceCollection _services;
        private readonly IConnectionsManager _manager;

        public LotosBuilder(IServiceCollection services)
        {
            _services = services;
            _manager = new ConnectionsManager();
        }

        public void Init()
        {
            InitConnectionsManager();
        }

        private void InitConnectionsManager()
        {
            _services.AddSingleton(_manager);
        }

        public LotosBuilder AttachDatabase<T>(T driver) where T : IDriver
        {
            var connection = driver.Run().GetAwaiter().GetResult();

            _manager.Add(connection);

            return this;
        }

        public LotosBuilder AttachDatabase<T>(T driver, out int pin) where T : IDriver
        {
            var connection = driver.Run().GetAwaiter().GetResult();

            pin = _manager.Add(connection);

            return this;
        }

        public LotosBuilder SingleDatabase<T>(T driver) where T : IDriver
        {
            var connection = driver.Run().GetAwaiter().GetResult();

            _services.AddSingleton(connection);

            return this;
        }
    }
}
