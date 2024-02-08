using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ExtensionMethods
{
    public static class CommonDependencyInject
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {

            return services;
        }
    }
}
