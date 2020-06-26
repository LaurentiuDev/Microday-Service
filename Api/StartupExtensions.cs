using Microsoft.Extensions.DependencyInjection;
using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Api
{
    internal static class StartupExtensions
    {
        /// <summary>
        /// Dynamically add all classes ending with Service as transient
        /// </summary>
        /// <param name="services"></param>
        internal static void RegisterServices(this IServiceCollection services)
        {
            AutoRegisterProcessors(services);
            AutoRegisterServices(services);
            //services.AddScoped<ExceptionsFilter>();
        }

        /// <summary>
        /// Dynamically add all Sieve processors as scoped services
        /// </summary>
        /// <param name="services"></param>
        internal static void AutoRegisterProcessors(IServiceCollection services)
        {
            var types = Assembly.GetExecutingAssembly().GetTypes();

            //  load all other services
            foreach (var service in types.Where(x => x.IsClass && !x.IsAbstract && (
                x.IsSubclassOf(typeof(SieveProcessor)) ||
                typeof(ISieveCustomSortMethods).IsAssignableFrom(x) ||
                typeof(ISieveCustomFilterMethods).IsAssignableFrom(x))))
            {
                services.AddScoped(service);
            }
        }

        /// <summary>
        /// Dynamically add all classes ending with Service as transient
        /// </summary>
        /// <param name="services"></param>
        internal static void AutoRegisterServices(IServiceCollection services)
        {
            var types = Assembly.GetExecutingAssembly().GetTypes();

            //  load all other services
            foreach (var service in types.Where(x => x.IsClass && !x.IsAbstract && x.FullName.EndsWith("Service", true, CultureInfo.InvariantCulture)))
            {
                services.AddTransient(service);
            }
        }
    }
}
