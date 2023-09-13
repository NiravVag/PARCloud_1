using Newtonsoft.Json;
using Par.CommandCenter.Application.Handlers.Controllers.Queries.GetControllersByTenant;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Par.CommandCenter.Application.Handlers.Controllers.Queries.GetController
{
    public class GetControllerResponse
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ControllerModel Controller { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<ControllerModel> Controllers { get; set; }
    }
}
