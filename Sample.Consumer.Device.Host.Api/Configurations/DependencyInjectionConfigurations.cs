using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sample.Consumer.Device.Infra.CrossCutting.DI;
using Sample.Consumer.Device.Infra.CrossCutting.EventHub.Extensions;

namespace Sample.Consumer.Device.Host.Api.Configurations
{
    internal static class DependencyInjectionConfigurations
    {
        public static IServiceCollection ConfigureDI(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddEventHub(configuration);

            DIFactory.ConfigureDI(services);

            return services;
        }
    }
}
