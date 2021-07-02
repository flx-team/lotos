using System;
using System.Threading.Tasks;
using Lotos.Abstractions.Database;
using Lotos.DependencyInjection.Common;
using Microsoft.Extensions.DependencyInjection;

namespace Lotos.DependencyInjection.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static LotosBuilder AddLotos(this IServiceCollection services)
        {
            var builder = new LotosBuilder(services);
            builder.Init();

            return builder;
        }
    }
}
