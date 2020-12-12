using System;
using System.Text.Json.Serialization;

namespace Sample.Consumer.Device.Infra.CrossCutting.Messages
{
    [Serializable]
    public class DeviceMessage
    {
        [JsonPropertyName("deviceCode")]
        public string DeviceCode { get; set; }

        [JsonPropertyName("deviceRegion")]
        public string DeviceRegion { get; set; }

        [JsonPropertyName("captureDate")]
        public DateTime? CaptureDate { get; set; }
    }
}
