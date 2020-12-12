using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sample.Consumer.Device.Infra.CrossCutting.EventHub.Options;
using Sample.Consumer.Device.Infra.CrossCutting.Options;

namespace Sample.Consumer.Device.Host.Api.Configurations
{
    internal static class AppSettingsOptionsConfigurations
    {
        private static string AppSettingsGlobalization => "Globalization";
        private static string AppSettingsEventHub => "EventHub";

        public static WebHostBuilderContext ConfigAppSettingsFiles(this WebHostBuilderContext hostingContext, IConfigurationBuilder configuration)
        {
            if (hostingContext.HostingEnvironment.EnvironmentName != "Development")
                configuration.AddJsonFile("appsettings/appsettings.json", optional: true, reloadOnChange: true);
            else
                configuration.AddJsonFile($"appsettings/appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true);

            configuration.AddEnvironmentVariables();

            return hostingContext;
        }

        public static IServiceCollection ConfigureAppSettingsOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();
            services.Configure<GlobalizationOptions>(options => configuration.GetSection(AppSettingsGlobalization).Bind(options));
            services.Configure<EventHubOptions>(options => configuration.GetSection(AppSettingsEventHub).Bind(options));

            return services;
        }
    }
}
