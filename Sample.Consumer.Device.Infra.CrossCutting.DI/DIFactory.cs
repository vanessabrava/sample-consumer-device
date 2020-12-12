using Microsoft.Extensions.DependencyInjection;
using Sample.Consumer.Device.Host.WebHosting;
using Sample.Consumer.Device.Services;
using Sample.Consumer.Device.Services.Imp;

namespace Sample.Consumer.Device.Infra.CrossCutting.DI
{
    public static class DIFactory
    {
        public static void ConfigureDI(IServiceCollection services)
        {
            //services.AddHttpContextAccessor();
            services.AddHostedService<DeviceHostService>();
            services.AddScoped<IDeviceService, DeviceService>();
        }
    }
}
