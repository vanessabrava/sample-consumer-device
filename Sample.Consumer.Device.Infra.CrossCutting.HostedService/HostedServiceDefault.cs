using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Sample.Consumer.Device.Infra.CrossCutting.HostedService
{
    public abstract class HostedServiceDefault<TStartup> : IHostedService
        where TStartup : class
    {
        protected HostedServiceDefault(IConfiguration configuration, IServiceProvider serviceProvider)
        {
            Configuration = configuration;
            ServiceProvider = serviceProvider;
        }

        protected IConfiguration Configuration { get; }

        protected IServiceProvider ServiceProvider { get; }

        public string Protocol { get; set; } = Guid.NewGuid().ToString("N");

        public virtual async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                await StartAction(cancellationToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public virtual async Task StopAsync(CancellationToken cancellationToken)
        {
            try
            {
                await Stop(cancellationToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }


        public abstract Task StartAction(CancellationToken cancellationToken);

        public abstract Task Stop(CancellationToken cancellationToken);
    }
}
