using Sample.Consumer.Device.Infra.CrossCutting.Messages;
using System.Threading.Tasks;

namespace Sample.Consumer.Device.Services
{
    public interface IDeviceService
    {
        Task ViewDeviceAsync(string protocol, DeviceMessage deviceMessage);
    }
}
