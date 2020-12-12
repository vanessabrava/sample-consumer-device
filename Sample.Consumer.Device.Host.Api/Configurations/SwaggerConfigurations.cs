using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Sample.Consumer.Device.Api.Filters;
using Sample.Microservice.Device.Api.Filters;
using System;

namespace Sample.Consumer.Device.Host.Api.Configurations
{
    internal static class SwaggerConfigurations
    {
        public static IServiceCollection ConfigureSwagger<TStartup>(this IServiceCollection services, string swaggerDocTitle, string swaggerDocVersion)
            where TStartup : class
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(swaggerDocVersion, new OpenApiInfo { Title = swaggerDocTitle, Version = swaggerDocVersion });
                c.IncludeXmlComments(GetXmlCommentsPath((typeof(TStartup).Assembly.GetName().Name)));
                c.IgnoreObsoleteActions();
                c.IgnoreObsoleteProperties();
                c.OperationFilter<SwaggerDefaultHeaderFilter>();
                c.DocumentFilter<SwaggerIgnoreOriginalHttpDocumentFilter>();
            });

            return services;
        }

        private static string GetXmlCommentsPath(string assemblyName) => $"{AppDomain.CurrentDomain.BaseDirectory}{assemblyName}.xml";
    }
}
