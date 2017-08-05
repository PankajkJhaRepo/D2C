using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulatedDevice
{
   public class DeviceReading
    {
        [JsonProperty("DeviceId")]
        public string DeviceId;

        [JsonProperty("Id")]
        public string Id;

        [JsonConverter(typeof(IsoDateTimeConverter))]
        [JsonProperty("ReadingTime")]
        public DateTime ReadingTime;

        [JsonProperty("Value")]
        public string Value;

        [JsonConverter(typeof(IsoDateTimeConverter))]
        [JsonProperty("QOutTime")]
        public DateTime QOutTime { get; set; }
    }
}
