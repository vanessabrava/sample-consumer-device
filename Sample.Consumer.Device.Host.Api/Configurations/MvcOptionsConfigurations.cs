using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Sample.Consumer.Device.Api.Binders;
using Sample.Consumer.Device.Api.Filters;

namespace Sample.Consumer.Device.Host.Api.Configurations
{
    internal static class MvcOptionsConfigurations
    {
        private static string AppSettingsHostToken => "Host:AuthorizationToken";

        public static MvcOptions ConfigureMvcOptions(this MvcOptions mvcOptions, IConfiguration configuration)
        {
            mvcOptions.ConfigureFilters(configuration);
            mvcOptions.ConfigureModelBinders();

            return mvcOptions;
        }

        private static MvcOptions ConfigureFilters(this MvcOptions options, IConfiguration configuration)
        {
            IConfigurationSection authorizationToken = configuration.GetSection(AppSettingsHostToken);

            options.Filters.Add(new AuthorizationTokenFilter(authorizationToken?.Value));
            options.Filters.Add(new RequestMessageFilterAttribute());
            options.Filters.Add(new RequestMessageValidationFilterAttribute());

            return options;
        }

        private static MvcOptions ConfigureModelBinders(this MvcOptions options)
        {
            options.ModelBinderProviders.Insert(0, new CurrentCultureDateTimeBinderProvider());
            options.ModelBinderProviders.Insert(1, new CurrentCultureDecimalBinderProvider());

            return options;
        }
    }
}
