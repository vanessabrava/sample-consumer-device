using Microsoft.Azure.EventHubs;
using Microsoft.Azure.EventHubs.Processor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sample.Consumer.Device.Infra.CrossCutting.EventHub;
using Sample.Consumer.Device.Infra.CrossCutting.EventHub.EventProcessor;
using Sample.Consumer.Device.Infra.CrossCutting.EventHub.Options;
using Sample.Consumer.Device.Infra.CrossCutting.Messages;
using Sample.Consumer.Device.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Sample.Consumer.Device.Host.EventHub
{
    public class DeviceEventProcessor : EventProcessorDefault
    {
        private IDeviceService DeviceService { get; }

        private IEventHubProducerService EventHubProducerService { get; }

        private int DegreeOfParallelism { get; } = Convert.ToInt32(Math.Ceiling(Environment.ProcessorCount * 0.85) * 2.0);

        public DeviceEventProcessor(ConsumerConfigurationsOptions consumerConfigurationsOptions, IServiceProvider serviceProvider, IConfiguration configuration)
            : base(consumerConfigurationsOptions, serviceProvider, configuration)
        {
            DeviceService = ServiceProvider.GetService<IDeviceService>();
            EventHubProducerService = ServiceProvider.GetService<IEventHubProducerService>();
        }

        public override async Task CloseAsync(PartitionContext context, CloseReason reason)
        {
            Console.WriteLine($"Processor Shutting Down. Partition '{context.PartitionId}', Reason: '{reason}'.");
            await base.CloseAsync(context, reason);
        }

        public override async Task OpenAsync(PartitionContext context)
        {
            Console.WriteLine($"DeviceEventProcessor initialized. Partition: '{context.PartitionId}'");
            await base.OpenAsync(context);
        }

        public override async Task ProcessErrorAsync(PartitionContext context, Exception error)
        {
            Console.WriteLine($"Error on Partition: {context.PartitionId}, Error: {error.Message}");
            await base.ProcessErrorAsync(context, error);
        }

        public override async Task<bool> ProcessEventDataList(PartitionContext context, List<EventData> eventDataList)
        {
            if (eventDataList == null || !eventDataList.Any())
                return false;

            var deviceMessageList = await GetDeviceMessage(context, eventDataList);

            if (deviceMessageList != null)
            {

                using (var tokenSource = new CancellationTokenSource())
                {
                    using SemaphoreSlim concurrencySemaphore = new SemaphoreSlim(DegreeOfParallelism, DegreeOfParallelism);
                    var tasks = new List<Task>();

                    foreach (var deviceMessage in deviceMessageList)
                    {
                        await concurrencySemaphore.WaitAsync();

                        tasks.Add(Task.Run(async () =>
                        {
                            try
                            {
                                await DeviceService.ViewDeviceAsync(deviceMessage.Key, deviceMessage.Value);
                            }
                            catch (Exception ex)
                            {
                                tokenSource.Cancel();
                                Console.WriteLine($"Error on Partition: {context.PartitionId}, Error: {ex.Message}");
                                throw;
                            }

                        }).ContinueWith(it =>
                        {
                            if (it.IsFaulted) throw it.Exception;
                            if (it.IsCompleted) concurrencySemaphore.Release();
                        }));
                    }

                    Task.WaitAll(tasks.ToArray());
                }
            }

            return await Task.FromResult(true);
        }

        public async Task<IDictionary<string, DeviceMessage>> GetDeviceMessage(PartitionContext context, List<EventData> eventDataList)
        {
            if (eventDataList == null || !eventDataList.Any())
                return default;

            var result = new Dictionary<string, DeviceMessage>();

            try
            {
                foreach (var eventData in eventDataList)
                {
                    Console.WriteLine($"Message received. Partition: '{context.PartitionId}', Data: '{eventData}'");
                    var device = await GetDeviceMessage(eventData);

                    result.Add(device.Key, device.Value);
                }
            }
            catch (Exception ex)
            {
                await EventHubProducerService.SendEventDataAsync("AuthorizerDeadLetter", eventDataList);
                Console.WriteLine($"Error on Partition: {context.PartitionId}, Error: {ex.Message}");
                return default;
            }

            return await Task.FromResult(result);
        }

        private async Task<KeyValuePair<string, DeviceMessage>> GetDeviceMessage(EventData eventData)
        {
            var json = Encoding.UTF8.GetString(eventData.Body.Array);

            if (eventData?.Properties == null)
                return await Task.FromResult(new KeyValuePair<string, DeviceMessage>(Guid.NewGuid().ToString("N"), null));

            if (eventData.Properties.TryGetValue("Protocol", out object protocol))
            {
                return await Task.FromResult(new KeyValuePair<string, DeviceMessage>((string)protocol, JsonSerializer.Deserialize<DeviceMessage>(json)));
            }

            return await Task.FromResult(new KeyValuePair<string, DeviceMessage>(Guid.NewGuid().ToString("N"), null));
        }

        public override Task ProcessEventsAsync(PartitionContext context, IEnumerable<EventData> messages)
        {
            return base.ProcessEventsAsync(context, messages);
        }
    }
}