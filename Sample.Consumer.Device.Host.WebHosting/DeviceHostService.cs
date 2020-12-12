using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sample.Consumer.Device.Host.EventHub;
using Sample.Consumer.Device.Infra.CrossCutting.EventHub;
using Sample.Consumer.Device.Infra.CrossCutting.HostedService;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Sample.Consumer.Device.Host.WebHosting
{
    public class DeviceHostService : HostedServiceDefault<DeviceHostService>
    {
        private IEventHubConsumerService EventHubConsumerService { get; }

        private string EventHubName => "SampleDeviceConsumer";

        private IServiceScope Scope { get; }

        public DeviceHostService(IConfiguration configuration, IServiceProvider serviceProvider)
            : base(configuration, serviceProvider)
        {
            Scope = serviceProvider.CreateScope();
            EventHubConsumerService = Scope.ServiceProvider.GetService<IEventHubConsumerService>();
        }

        public override async Task StartAction(CancellationToken cancellationToken)
        {
            await EventHubConsumerService.RegisterEventMessageConsumerAsync<DeviceEventProcessor>(EventHubName);
        }

        public override async Task Stop(CancellationToken cancellationToken)
        {
            await EventHubConsumerService.UnregisterEventMessageConsumerAsync(EventHubName);
        }
    }
}