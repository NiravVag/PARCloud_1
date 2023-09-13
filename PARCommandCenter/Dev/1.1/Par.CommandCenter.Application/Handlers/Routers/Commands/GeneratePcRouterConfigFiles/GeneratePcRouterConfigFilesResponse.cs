using Par.CommandCenter.Application.Common.Utilities;
using Par.CommandCenter.Domain.Model;

namespace Par.CommandCenter.Application.Handlers.Routers.Commands.GeneratePcRouterConfigFiles
{
    public class GeneratePcRouterConfigFilesResponse
    {
        public int RouterId { get; set; }

        public string RouterAddress { get; set; }


        public CcFile CloudRouterConfigZipFile { get; set; }
    }
}
