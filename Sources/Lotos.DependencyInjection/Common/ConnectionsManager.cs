using System;
using System.Collections.Generic;
using Lotos.Abstractions.Database;

namespace Lotos.DependencyInjection.Common
{
    internal sealed class ConnectionsManager : IConnectionsManager
    {
        private int _lastPin = 0;
        private readonly Dictionary<int, IConnection> _pins = new();

        public int Add(IConnection connection)
        {
            _pins.Add(_lastPin, connection);

            _lastPin++;

            return _lastPin;
        }

        public void Remove(int pin)
        {
            _pins.Remove(pin);
        }

        public IConnection Get(int pin)
        {
            return _pins[pin];
        }

        public void Dispose()
        {
            foreach (var element in _pins)
            {
                element.Value.Dispose();

                _pins.Remove(element.Key);
            }
        }
    }
}
