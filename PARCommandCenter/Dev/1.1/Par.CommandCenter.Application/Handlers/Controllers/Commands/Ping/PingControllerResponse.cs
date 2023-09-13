using Newtonsoft.Json;
using Par.CommandCenter.Domain.Model;

namespace Par.CommandCenter.Application.Handlers.Controllers.Commands.Ping
{
    public class PingControllerResponse
    {
        public bool Success { get; set; }

        public PingCloudControllerResponse PingResponse { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Message { get; set; }
    }
}
