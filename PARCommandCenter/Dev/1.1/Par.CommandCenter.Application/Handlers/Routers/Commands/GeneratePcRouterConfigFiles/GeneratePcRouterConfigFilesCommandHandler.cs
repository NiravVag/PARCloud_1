using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Par.CommandCenter.Application.Common.Utilities;
using Par.CommandCenter.Application.Handlers.Routers.Queries.GetRoutersByTenant;
using Par.CommandCenter.Application.Interfaces;
using Par.CommandCenter.Domain.Entities;
using Par.CommandCenter.Domain.Enums;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Par.CommandCenter.Domain.Model;

namespace Par.CommandCenter.Application.Handlers.Routers.Commands.GeneratePcRouterConfigFiles
{
    public class GeneratePcRouterConfigFilesCommandHandler : IRequestHandler<GeneratePcRouterConfigFilesCommand, GeneratePcRouterConfigFilesResponse>
    {
        private readonly ILogger<GeneratePcRouterConfigFilesCommandHandler> _logger;
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;


        public GeneratePcRouterConfigFilesCommandHandler(IApplicationDbContext dbContext, ICurrentUserService currentUserService, IMapper mapper, ILogger<GeneratePcRouterConfigFilesCommandHandler> logger)
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<GeneratePcRouterConfigFilesResponse> Handle(GeneratePcRouterConfigFilesCommand request, CancellationToken cancellationToken)
        {
            if(!request.ControllerIds?.Any() ?? false)
            {
                throw new ArgumentException($"You must proivde at least one controller Id");
            }
           
            var query = from c in _dbContext.Controllers
                        join r in _dbContext.Routers on c.RouterId equals r.Id
                        where request.ControllerIds.Contains(c.Id)
                        select new Controller ()
                        {
                            Id = c.Id, 
                            IpAddress = c.IpAddress,
                            NetworkPort = c.NetworkPort,
                            PortName = c.PortName,
                            Router = new Router()
                            {
                                Id =r.Id,
                                Address = r.Address,
                                ComputerName = r.ComputerName,
                            }
                        };

            var controllers = await query
                .OrderBy(x => x.NetworkPort).ToListAsync();

            if(!controllers.Any())
            {
                throw new ArgumentException($"The provided controller Ids is not avaiable or deleted.");
            }

            IList<string> tag1ValueList = new List<string>();
            IList<string> tag2ValueList = new List<string>();
            IList<string> tag3ValueList = new List<string>();

            int index = 0;

            foreach (var controller in controllers)
            {

                if(string.IsNullOrWhiteSpace(controller.IpAddress) || string.IsNullOrWhiteSpace(controller.NetworkPort.ToString()))
                {
                    continue;
                }

                tag1ValueList.Add($"COW<{controller.IpAddress}:{controller.NetworkPort.ToString()}>[ParCharge]");
                if (index <= 7)
                {
                    tag2ValueList.Add(@$"{"{"}""Network"":""{controller.IpAddress}:{controller.NetworkPort.ToString()}"",""USB"":""{controller.PortName.Trim().Replace("COW", "PBS")}""{"}"}");
                } else
                {
                    tag3ValueList.Add(@$"{"{"}""Network"":""{controller.IpAddress}:{controller.NetworkPort.ToString()}"",""USB"":""{controller.PortName.Trim().Replace("COW", "PBS")}""{"}"}");
                }

                index++;
            }

            var filesList = new List<CcFile>();

            var tag1Value = string.Join(",\n", tag1ValueList);
            var tag2Value = string.Join(",\n", tag2ValueList);

           

            TemplateParser tp = new TemplateParser();
            tp.AddTag(new TemplateTag("[%tag1%]", tag1Value));
            tp.AddTag(new TemplateTag("[%tag2%]", tag2Value));

            if(controllers.Count > 8)
            {
                var tag3Value = string.Join(",\n", tag3ValueList);
                tp.AddTag(new TemplateTag("[%tag3%]", tag3Value));
            }
            

            tp.AddTag(new TemplateTag("[%tag4%]", controllers.FirstOrDefault()?.Router?.ComputerName?.Trim()));
            tp.AddTag(new TemplateTag("[%tag5%]", controllers.FirstOrDefault()?.Router?.Address?.Trim()));


            string templateFilename = $"{AppDomain.CurrentDomain.BaseDirectory}\\Resources\\PcRouterConfigurations\\CloudRouter.cfg";
            if (!System.IO.File.Exists(templateFilename)) throw new FileNotFoundException(nameof(templateFilename), "File does not exist."); 
            string parsedFile = tp.ParseTemplateFile(templateFilename);
            byte[] byteArray = Encoding.ASCII.GetBytes(parsedFile);
            CcFile cloudRouterConfigFile = new CcFile(byteArray, "CloudRouter", "text/plain", "cfg");
            filesList.Add(cloudRouterConfigFile);

           
            templateFilename = $"{AppDomain.CurrentDomain.BaseDirectory}\\Resources\\PcRouterConfigurations\\cowserver_1.cfg";
            if (!System.IO.File.Exists(templateFilename)) throw new FileNotFoundException(nameof(templateFilename), "File does not exist.");
            parsedFile = tp.ParseTemplateFile(templateFilename);
            byteArray = Encoding.ASCII.GetBytes(parsedFile);
            cloudRouterConfigFile = new CcFile(byteArray, "cowserver_1", "text/plain", "cfg");
            filesList.Add(cloudRouterConfigFile);

            if (controllers.Count > 8)
            {
                templateFilename = $"{AppDomain.CurrentDomain.BaseDirectory}\\Resources\\PcRouterConfigurations\\cowserver_2.cfg";
                if (!System.IO.File.Exists(templateFilename)) throw new FileNotFoundException(nameof(templateFilename), "File does not exist.");
                parsedFile = tp.ParseTemplateFile(templateFilename);
                byteArray = Encoding.ASCII.GetBytes(parsedFile);
                cloudRouterConfigFile = new CcFile(byteArray, "cowserver_2", "text/plain", "cfg");
                filesList.Add(cloudRouterConfigFile);
            }

            templateFilename = $"{AppDomain.CurrentDomain.BaseDirectory}\\Resources\\PcRouterConfigurations\\PSSTART1.bat";
            if (!System.IO.File.Exists(templateFilename)) throw new FileNotFoundException(nameof(templateFilename), "File does not exist.");
            parsedFile = tp.ParseTemplateFile(templateFilename);
            byteArray = Encoding.ASCII.GetBytes(parsedFile);
            cloudRouterConfigFile = new CcFile(byteArray, "PSSTART1", "text/plain", "bat");
            filesList.Add(cloudRouterConfigFile);

            if(filesList.Any())
            {
                var zipFile = ZipFileHelper.ArchiveFileList(filesList);

                return new GeneratePcRouterConfigFilesResponse()
                {                    
                    CloudRouterConfigZipFile = zipFile
                };
            }

            return null;
        }
    }
}
