using Sample.Consumer.Device.Infra.CrossCutting.Messages;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Sample.Consumer.Device.Services.Imp
{
    public class DeviceService : IDeviceService
    {
        public async Task ViewDeviceAsync(string protocol, DeviceMessage deviceMessage)
        {
            Console.WriteLine($"Protocol '{protocol}', Device: '{JsonSerializer.Serialize(deviceMessage)}'");
            await Task.CompletedTask;
        }
    }
}
