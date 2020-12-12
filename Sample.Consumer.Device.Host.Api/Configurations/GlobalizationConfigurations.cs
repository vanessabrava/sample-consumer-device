using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Sample.Consumer.Device.Infra.CrossCutting.Options;
using System.Globalization;
using System.Linq;

namespace Sample.Consumer.Device.Host.Api.Configurations
{
    internal static class GlobalizationConfigurations
    {
        private static string AppSettingsGlobalization => "Globalization";

        public static IApplicationBuilder UseGlobalization(this IApplicationBuilder applicationBuilder, IConfiguration configuration)
        {
            var globalizationOptions = new GlobalizationOptions();
            configuration.GetSection(AppSettingsGlobalization).Bind(globalizationOptions);

            applicationBuilder.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(globalizationOptions?.DefaultRequestCulture, globalizationOptions?.DefaultRequestCulture),
                SupportedCultures = globalizationOptions?.SupportedCultures.Select(it => new CultureInfo(it)).ToList(),
                SupportedUICultures = globalizationOptions?.SupportedUICultures.Select(it => new CultureInfo(it)).ToList(),
            });

            return applicationBuilder;
        }
    }
}