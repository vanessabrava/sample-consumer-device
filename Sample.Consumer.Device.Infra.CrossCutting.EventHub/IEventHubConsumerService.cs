﻿using Microsoft.Azure.EventHubs.Processor;
using Sample.Consumer.Device.Infra.CrossCutting.EventHub.EventProcessor;
using System.Threading.Tasks;

namespace Sample.Consumer.Device.Infra.CrossCutting.EventHub
{

    public interface IEventHubConsumerService
    {
       
        Task<bool> RegisterEventMessageConsumerAsync<TEventProcessor>(string name) where TEventProcessor : EventProcessorDefault;

        Task<bool> RegisterEventMessageConsumerAsync(string name, IEventProcessorFactory eventProcessorFactory);

        Task<IEventProcessorFactory> UnregisterEventMessageConsumerAsync(string name);

        Task UnregisterAllMessageConsumerAsync();
    }
}